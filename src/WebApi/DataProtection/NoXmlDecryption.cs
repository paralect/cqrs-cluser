namespace WebApi.DataProtection
{
    using System.Xml.Linq;
    using Microsoft.AspNetCore.DataProtection.XmlEncryption;

    public class NoXmlDecryption : IXmlDecryptor
    {
        public XElement Decrypt(XElement encryptedElement)
        {
            return encryptedElement;
        }
    }
}
