namespace WebApi.DataProtection
{
    using Microsoft.AspNetCore.DataProtection;

    public class PassthroughDataProtector : IDataProtector
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return new PassthroughDataProtector();
        }

        public byte[] Protect(byte[] plaintext)
        {
            return plaintext;
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return protectedData;
        }
    }
}
