namespace Project.UseCases
{
    public interface IGeneralRepository
    {
        public string HashPassword(string Password, out byte[] salt);
        public bool ComparePassword(string Password, string pwHashed, string salt);
    }
}