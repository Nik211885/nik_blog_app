namespace Application.Adapters;
public interface ITokenEncryptionService
{
    /// <summary>
    ///  Encrypt data is json string 
    /// </summary>
    /// <param name="plainText">data</param>
    /// <returns>
    ///     Base 64 after encrypt data
    /// </returns>
    string Encrypt(string plainText);
    /// <summary>
    ///  Decrypt and get data is json string 
    /// </summary>
    /// <param name="cipherText">data before encrypt</param>
    /// <returns>
    ///  String json data plain text      
    /// </returns>
    string Decrypt(string cipherText);
}