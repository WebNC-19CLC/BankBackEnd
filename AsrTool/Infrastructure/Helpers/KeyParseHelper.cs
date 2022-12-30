using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AsrTool.Infrastructure.Domain;

namespace AsrTool.Infrastructure.Helpers
{
  public static class KeyParseHelper
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
            return (RSAParameters)xs.Deserialize(sr);
        }
  }
}
