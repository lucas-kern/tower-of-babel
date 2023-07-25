using System;

// This class represents a User
[Serializable]
public class User
{
    public int UserId;
    public string first_name;
    public string last_name;
    public string password;
    public string email;
    public string Token;
    public string refreshToken;
    public bool Completed;
}
