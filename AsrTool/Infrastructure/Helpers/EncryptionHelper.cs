using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AsrTool.Infrastructure.Domain;

namespace AsrTool.Infrastructure.Helpers
{
  public static class EncryptionHelper
  {
        public static string ConvertRSAKeyToString(RSAParameters key)
        {
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs.Serialize(sw, key);
            //get the string from the stream
            return sw.ToString();
        }

        public static RSAParameters ConvertStringToRSAKey(string pubKeyString)
        {
            //get a stream from the string
            var sr = new System.IO.StringReader(pubKeyString);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            var publicKey =  (RSAParameters)xs.Deserialize(sr);

            return publicKey;
        }

        public static string RSAEncryption(string plainText, RSAParameters rsaPublicKey)
        {
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaPublicKey);

            var bytesPlainTextData = Encoding.Unicode.GetBytes(plainText);

            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            var cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }

        public static string RSADecryption(string cypherText, RSAParameters rsaPrivateKey)
        {
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaPrivateKey);

            var bytesCypherText = Convert.FromBase64String(cypherText);

            var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            var plainText = Encoding.Unicode.GetString(bytesPlainTextData);

            return plainText;
        }

        public static string ComputeHash(String secretKey, String authenticationDataString)
        {
            HMACSHA512 hmac = new HMACSHA512(Encoding.ASCII.GetBytes(secretKey));

            Byte[] authenticationData = UTF8Encoding.GetEncoding("utf-8").GetBytes(authenticationDataString);

            var hashedToken = hmac.ComputeHash(authenticationData);
            return Convert.ToBase64String(hashedToken);
        }


    }
}
