namespace WebApi.DataProtection
{
    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

    public class NoEncryptionConfiguration : IAuthenticatedEncryptorConfiguration
    {
        public IAuthenticatedEncryptorDescriptor CreateNewDescriptor()
        {
            return new NoEncryptionDescriptor();
        }
    }
}
