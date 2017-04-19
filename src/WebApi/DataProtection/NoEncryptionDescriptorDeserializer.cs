namespace WebApi.DataProtection
{
    using System.Xml.Linq;
    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

    public class NoEncryptionDescriptorDeserializer : IAuthenticatedEncryptorDescriptorDeserializer
    {
        public IAuthenticatedEncryptorDescriptor ImportFromXml(XElement element)
        {
            return new NoEncryptionDescriptor();
        }
    }
}