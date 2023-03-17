using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a User
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public string refreshToken { get; set; }
    public bool Completed { get; set; }
}
