namespace WebApi.DataProtection
{
    using Microsoft.AspNetCore.DataProtection;

    public class PassthroughDataProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return new PassthroughDataProtector();
        }
    }
}
