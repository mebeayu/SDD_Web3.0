using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Common
{
    public class SerializeHelper
    {
        public static string Serialize2XML<T>(object o, bool isConcise)
        {

            //去掉默认的命名空间与前缀
            if (isConcise)
            {
                StringBuilder sb = new StringBuilder();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    XmlSerializer xz = new XmlSerializer(typeof(T));
                    xz.Serialize(writer, o, ns);
                }
                return sb.ToString();
            }

            return Serialize2XML<T>(o);
        }
        public static string Serialize2XML<T>(object o)
        {
            return Serialize2XML(typeof(T), o);
        }
        public static string Serialize2XML(Type type, object o)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(type);
                xz.Serialize(sw, o);
                return sw.ToString();
            }
        }
        public static object DeserializeFromXML(Type type, string xml)
        {
            using (StringReader rdr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(rdr);
            }
        }
        public static T DeserializeFromXML<T>(string xml)
        {
            Type type = typeof(T);
            return (T)DeserializeFromXML(type, xml);
        }

        public static string BinarySerialize2Base64(object o)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, o);
            ms.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        public static object BinaryDeserializeFromBase64(string base64str)
        {
            object o = null;

            MemoryStream ms = null;
            if (base64str != null && base64str.Trim().Length > 0)
                ms = new MemoryStream(Convert.FromBase64String(base64str));

            if (ms != null && ms.Length > 0)
            {
                IFormatter formatter = new BinaryFormatter();
                o = formatter.Deserialize(ms);
            }

            return o;
        }

        public static List<string> ForceParseString(List<string> srcLst)
        {
            List<string> result = new List<string>();

            foreach (var item in srcLst)
            {
                string[] split = item.Split(",".ToCharArray());
                foreach (string s in split)
                    if (s != null && s.Trim().Length > 0)
                        result.Add(s.Trim());
            }

            return result;
        }
    }
}
