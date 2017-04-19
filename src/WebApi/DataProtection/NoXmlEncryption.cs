namespace WebApi.DataProtection
{
    using System.Xml.Linq;
    using Microsoft.AspNetCore.DataProtection.XmlEncryption;

    public class NoXmlEncryption : IXmlEncryptor
    {
        public EncryptedXmlInfo Encrypt(XElement plaintextElement)
        {
            return new EncryptedXmlInfo(plaintextElement, typeof(NoXmlDecryption));
        }
    }
}
