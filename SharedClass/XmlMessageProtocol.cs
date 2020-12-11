

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SharedClass
{
    public class XmlMessageProtocol : Protocol<XDocument>
    {
        protected override XDocument Decode(byte[] message)
        {
            var xmlData = Encoding.UTF8.GetString(message);
            var xmlReader = XmlReader.Create(new StringReader(xmlData), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });
            return XDocument.Load(xmlReader);
        }
        protected override byte[] EncodeBody<T>(T message)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(sw, message);
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
