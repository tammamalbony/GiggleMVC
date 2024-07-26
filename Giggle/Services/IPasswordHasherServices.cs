namespace Giggle.Services
{
    public interface IPasswordHasherServices
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
