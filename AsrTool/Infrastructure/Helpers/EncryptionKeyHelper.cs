using System.Reflection;
using System.Security.Cryptography;
using AsrTool.Infrastructure.Domain.Objects.Configurations;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AsrTool.Infrastructure.Helpers
{
  public static class EncryptionKeyHelper
  {
    private static readonly AppSettings _settings = GetSettings();
    private static readonly IReadOnlyDictionary<(string entityName, string colName), string> _encryptedColumns =
      GetEncryptedColumns(new List<(string entityName, string colName)>()
      {
        // TO ADD FIELDS THAT NEED TO BE ENCRYPTED
        //(nameof(Employee), nameof(Employee.BirthDate))
      });

    private static AppSettings GetSettings()
    {
      var builder = new ConfigurationBuilder();
      builder.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.overrides.json", optional: false, reloadOnChange: true);

      var config = builder.Build();
      return config.GetSection(nameof(AppSettings)).Get<AppSettings>();
    }

    private static Dictionary<(string entityName, string colName), string> GetEncryptedColumns(List<(string entityName, string colName)> columns)
    {
      var encryptedColumns = new Dictionary<(string, string), string>();
      columns.ForEach(x =>
      {
        encryptedColumns.Add((x.entityName, x.colName), $"{x.entityName}_{x.colName}".ToUpper());
      });
      return encryptedColumns;
    }

    private static void CreateMasterKey(AppSettings config)
    {
      var masterKeyName = $"CMK_{Guid.NewGuid().ToString().Replace("-", "_")}";
      var masterKeySql = @$"
        IF NOT EXISTS (SELECT 1
          FROM [sys].[column_master_keys])
        BEGIN
          CREATE COLUMN MASTER KEY {masterKeyName} WITH (
            KEY_STORE_PROVIDER_NAME = '{Constants.AlwaysEncrypted.KeyStoreProviderName}',
            KEY_PATH = N'{config.KeyPath}')
        END;";
      using var connection = new SqlConnection(config.AsrToolDbConnectionString);
      connection.ExecuteScalar(masterKeySql);
    }

    private static void CreateEncryptionKeys(AppSettings config)
    {
      var provider = new SqlColumnEncryptionCngProvider();

      using var connection = new SqlConnection(config.AsrToolDbConnectionString);
      var masterKeyName = connection.Query<(string name, int id)>($@"
        SELECT name, column_master_key_id
        FROM [sys].[column_master_keys]
        WHERE key_path = '{config.KeyPath}'")
        .FirstOrDefault()
        .name;

      if (masterKeyName == default)
      {
        return;
      }

      var encryptionAlgorithm = Constants.AlwaysEncrypted.KeyEncryptionAlgorithm;
      using var rng = new RNGCryptoServiceProvider();

      foreach (var columnName in _encryptedColumns.Values)
      {
        var randomBytes = new byte[32];
        rng.GetBytes(randomBytes);
        var encryptedKey = provider.EncryptColumnEncryptionKey(config.KeyPath, encryptionAlgorithm, randomBytes);
        var encryptedValue = "0x" + BitConverter.ToString(encryptedKey).Replace("-", "");

        var encryptionKeySql = @$"
          IF NOT EXISTS (SELECT 1
            FROM [sys].[column_encryption_keys]
            WHERE [name] = 'CEK_{columnName}')
          BEGIN
            CREATE COLUMN ENCRYPTION KEY CEK_{columnName} WITH VALUES (
              COLUMN_MASTER_KEY = {masterKeyName},
              ALGORITHM = '{encryptionAlgorithm}',
              ENCRYPTED_VALUE = {encryptedValue})
          END;";
        connection.ExecuteScalar(encryptionKeySql);
      }
    }

    public static void CreateKeys(AppSettings config)
    {
      CreateMasterKey(config);
      CreateEncryptionKeys(config);
    }

    public struct ColumnEncryptionKeyData
    {
      public enum EncryptionType
      {
        DETERMINISTIC,
        RANDOMIZED
      };
      public string EntityName;
      public string PropertyName;
      public string PropertyType;
      public EncryptionType Type;
      public bool Nullable;
      public string DefaultValue;
    }

    public static List<string> BuildEncryptionColumnsSql(params ColumnEncryptionKeyData[] dataSet)
    {
      var encrypt = _settings.ActivateEncryption;

      var result = new List<string>();

      foreach (var data in dataSet)
      {
        var encryptionKeyName = _encryptedColumns.GetValueOrDefault((data.EntityName, data.PropertyName));
        if (encrypt && encryptionKeyName == default)
        {
          continue;
        }

        string encryptionKeyNameWithPrefix = $"CEK_{encryptionKeyName}";
        string[] collationDataTypes = { "[char]", "[varchar]", "[nchar]", "[nvarchar]", "[text]", "[ntext]" };
        string collation = data.Type == ColumnEncryptionKeyData.EncryptionType.DETERMINISTIC && collationDataTypes.Any(x => data.PropertyType.ToLower().StartsWith(x))
          ? "COLLATE Latin1_General_BIN2"
          : "";
        string encryptionColumnsSql = @$"
          ALTER TABLE [dbo].[{data.EntityName}] ADD
          [{data.PropertyName}] {data.PropertyType}
          { (encrypt ? @$"{collation}
          ENCRYPTED WITH(
            ENCRYPTION_TYPE = { data.Type}, 
            ALGORITHM = '{Constants.AlwaysEncrypted.DataEncryptionAlgorithm}', 
            COLUMN_ENCRYPTION_KEY = { encryptionKeyNameWithPrefix})" : "")}
          {(data.Nullable ? "" : "NOT NULL")}
          {(data.DefaultValue == default ? "" : $"DEFAULT({data.DefaultValue})")}";

        result.Add(encryptionColumnsSql);
      }

      return result;
    }
  }
}
