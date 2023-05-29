namespace Project.RSA
{
    public interface IRsaService
    {
        string Encrypt(string text);
        string Decrypt(string encrypted);
        string getPublicKey();

    }
}