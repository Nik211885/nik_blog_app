using Application.Adapters;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Adapters;
public class TokenEncryptionService : ITokenEncryptionService
{
    private readonly byte[] _tokenSignatureKey;
    public TokenEncryptionService(IConfiguration configuration)
    {
        string? tokenSignature = configuration.GetSection("TokenSignature").Get<string>();
        ArgumentNullException.ThrowIfNull(tokenSignature);
        _tokenSignatureKey = Encoding.UTF8.GetBytes(tokenSignature);
    }
    /// <summary>
    ///  Encrypt data is json string 
    /// </summary>
    /// <param name="plainText">data</param>
    /// <returns>
    ///     Base 64 after encrypt data
    /// </returns>
    public string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        using Aes aes = Aes.Create();
        aes.Key = _tokenSignatureKey;

        byte[] iv = new byte[aes.BlockSize / 8];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
        aes.IV = iv;
        using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        byte[] bytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Encoding.UTF8.GetString(bytes);
    }
    /// <summary>
    ///  Decrypt and get data is json string 
    /// </summary>
    /// <param name="cipherText">data before encrypt</param>
    /// <returns>
    ///  String json data plain text      
    /// </returns>
    public string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = _tokenSignatureKey;
        aes.GenerateIV();
        byte[] iv = aes.IV;
        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipher = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

        byte[] result = new byte[iv.Length + cipher.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(cipher, 0, result, iv.Length, cipher.Length);
        return Convert.ToBase64String(result);
    }
}