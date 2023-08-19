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
    // TODO Define meta properties if needed
}