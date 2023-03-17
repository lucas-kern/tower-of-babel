using System;
using UnityEngine;

// This class handles serialization regarding JSON
// example serialization and deserialization

// TODO add serialization to send json in an API request

public class JsonSerializationOption : ISerializationOption
{
    public string ContentType => "application/json";
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
}
