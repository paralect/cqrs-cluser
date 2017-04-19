namespace WebApi.DataProtection
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

    public class NoEncryption : IAuthenticatedEncryptor
    {
        public byte[] Decrypt(ArraySegment<byte> ciphertext, ArraySegment<byte> additionalAuthenticatedData)
        {
            return ciphertext.Array
                .Skip(ciphertext.Offset)
                .ToArray();
        }

        public byte[] Encrypt(ArraySegment<byte> plaintext, ArraySegment<byte> additionalAuthenticatedData)
        {
            return plaintext.Array
                .Skip(plaintext.Offset)
                .ToArray();
        }
    }
}
