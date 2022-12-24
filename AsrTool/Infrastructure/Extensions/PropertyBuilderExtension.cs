using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace AsrTool.Infrastructure.Extensions
{
  public static class PropertyBuilderExtension
  {
    public static PropertyBuilder HasUnicodeTextColumn(this PropertyBuilder propertyBuilder, int maxLength)
    {
      return propertyBuilder.HasColumnType("nvarchar").HasMaxLength(maxLength);
    }

    public static PropertyBuilder HasMaxUnicodeTextColumn(this PropertyBuilder propertyBuilder)
    {
      return propertyBuilder.HasColumnType("nvarchar(MAX)");
    }

    public static PropertyBuilder HasAsciiColumn(this PropertyBuilder propertyBuilder, int maxLength)
    {
      return propertyBuilder.HasColumnType("varchar").HasMaxLength(maxLength);
    }

    public static PropertyBuilder HasMaxAsciiColumn(this PropertyBuilder propertyBuilder)
    {
      return propertyBuilder.HasColumnType("varchar(MAX)");
    }

    public static PropertyBuilder HasObjectToStringConversion<TModel>(this PropertyBuilder<TModel> propertyBuilder)
      where TModel : class
    {
      return propertyBuilder.HasConversion(
        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        v => JsonConvert.DeserializeObject<TModel>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        new ValueComparer<TModel>(
          (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
          c => c == null ? 0 : JsonConvert.SerializeObject(c).GetHashCode(),
          c => JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(c))));
    }

    public static PropertyBuilder HasEnumCollectionToStringConversion<TModel>(this PropertyBuilder<ICollection<TModel>> propertyBuilder)
      where TModel : Enum
    {
      return propertyBuilder.HasConversion(
        v => JsonConvert.SerializeObject(v.Select(x => x.ToString()), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        v => JsonConvert.DeserializeObject<ICollection<TModel>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        new ValueComparer<ICollection<TModel>>(
          (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
          c => c == null ? 0 : JsonConvert.SerializeObject(c).GetHashCode(),
          c => JsonConvert.DeserializeObject<ICollection<TModel>>(JsonConvert.SerializeObject(c))));
    }

    public static PropertyBuilder HasCollectionToStringConversion<TModel>(this PropertyBuilder<ICollection<TModel>> propertyBuilder)
      where TModel : class
    {
      return propertyBuilder.HasConversion(
        v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        v => JsonConvert.DeserializeObject<ICollection<TModel>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
        new ValueComparer<ICollection<TModel>>(
          (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
          c => c == null ? 0 : JsonConvert.SerializeObject(c).GetHashCode(),
          c => JsonConvert.DeserializeObject<ICollection<TModel>>(JsonConvert.SerializeObject(c))));
    }

    public static PropertyBuilder HasEnumToStringConversation<TEnum>(this PropertyBuilder<TEnum> propertyBuilder)
      where TEnum : struct
    {
      return propertyBuilder.HasConversion(new EnumToStringConverter<TEnum>());
    }

    public static PropertyBuilder HasComputedDateDiffColumn<T>(
      this PropertyBuilder<T> propertyBuilder, DateDiffType dateType, string dateFromPropertyName)
    {
      var sqlType = ConvertToSqlType(dateType);
      return propertyBuilder.HasComputedColumnSql(
        @$"CASE 
	        WHEN [{dateFromPropertyName}] IS NULL THEN NULL
	        WHEN DATEDIFF({sqlType}, [{dateFromPropertyName}], GETDATE()) < 0 THEN NULL
	        ELSE DATEDIFF({sqlType}, [{dateFromPropertyName}], GETDATE())
        END");
    }

    public static PropertyBuilder HasComputedDateDiffColumn<T>(
      this PropertyBuilder<T> propertyBuilder, DateDiffType dateType, string dateFromPropertyName, string dateToPropertyName)
    {
      var sqlType = ConvertToSqlType(dateType);
      return propertyBuilder.HasComputedColumnSql(
        @$"CASE 
	        WHEN [{dateFromPropertyName}] IS NULL THEN NULL
	        WHEN DATEDIFF({sqlType}, [{dateFromPropertyName}], [{dateToPropertyName}]) < 0 THEN NULL
	        ELSE DATEDIFF({sqlType}, [{dateFromPropertyName}], [{dateToPropertyName}])
        END");
    }

    private static string ConvertToSqlType(DateDiffType type)
    {
      return type switch
      {
        DateDiffType.Day => "DAY",
        DateDiffType.Month => "MONTH",
        DateDiffType.Year => "YEAR",
        _ => throw new NotSupportedException($"Type {type} is not supported")
      };
    }

    public enum DateDiffType
    {
      Year,
      Month,
      Day,
    }
  }
}
