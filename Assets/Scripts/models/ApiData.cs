using System;

[Serializable]
public class ApiResponse<T>
{
    public MetaData meta;
    public T data;
}

[Serializable]
public class MetaData
{
     public int statusCode; 
}

[Serializable]
public class TokenRefreshData
{
    public string userId;
    public string refreshToken;
    public string token;
}
