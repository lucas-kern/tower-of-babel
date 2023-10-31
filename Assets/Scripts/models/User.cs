using System;

// This class represents a User
[Serializable]
public class User
{
    public string user_id;
    public string first_name;
    public string last_name;
    public string password;
    public string email;
    public string token;
    public string refresh_token;
    public bool Completed;
    public Base Base;
}
