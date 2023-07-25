using System;
using UnityEngine;

// This class handles serialization regarding JSON
// example serialization and deserialization

// TODO add serialization to send json in an API request

public class JsonSerializationOption : ISerializationOption
{
    // JSON content type for JSON serialization option
    public string ContentType => "application/json";

    //Deserialize a JSON string to a c# object
    public T Deserialize<T>(string text)
    {
        try
        {
            var result = JsonUtility.FromJson<T>(text);
            Debug.Log($"Success: {text}");
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError($"Could not parse response {text}. {ex.Message}");
            return default;
        }
    }

// Serialize an object as a JSON string
    public string Serialize<T>(T obj)
    {
        try
        {
            var result = JsonUtility.ToJson(obj);
            Debug.Log($"Success: {result}");
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError($"Could not parse object {typeof(T).FullName}. {ex.Message}");
            return default;
        }
    }
}
