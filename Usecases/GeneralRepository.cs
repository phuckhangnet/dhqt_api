using Project.Data;
using System.Security.Cryptography;
using System.Text;
namespace Project.UseCases
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly DataContext _dbContext;
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public GeneralRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string HashPassword(string Password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(Password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray();
        }
        public bool ComparePassword(string password, string hash, string _salt)
        {
            try 
            {
                byte[] salt = StringToByteArray(_salt);
                var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
                return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
            }
            catch {
                return false;
            }
        }
        
    }
}