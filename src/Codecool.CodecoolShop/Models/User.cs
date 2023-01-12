namespace Codecool.CodecoolShop.Models;

public class User
{
    public string UserName { get; }
    public string Email { get; }
    public HashSalt Password { get; }
    
    public User(string userName, string email, HashSalt password)
    {
        UserName = userName;
        Password = password;
        Email = email;
    }
}