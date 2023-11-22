using System;

// This class represents a User
[Serializable]
public class User
{
    public string userId;
    public string firstName;
    public string lastName;
    public string password;
    public string email;
    public string token;
    public string refreshToken;
    public bool Completed;
    public Base Base;
}
