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
    // TODO Define meta properties if needed
}

[Serializable]
public class TokenRefreshData
{
    public string user_id;
    public string refresh_token;
    public string token;
}
