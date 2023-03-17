using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// This class handles all interactions with servers 
public class NetworkManager
{
    // The serialization option to serialize and deserialize for requests
    private readonly ISerializationOption _serializationOption;
    //Instantiates a NetworkManager to handle API requests
    public NetworkManager(ISerializationOption serializationOption)
    {
        _serializationOption = serializationOption;
    }

// Make a GET request to an API
    public async Task<TResultType> Get<TResultType>(string url)
    {
        try
        {
            using var uwr = UnityWebRequest.Get(url);

            uwr.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            var operation = uwr.SendWebRequest();

            while(!operation.isDone)
                await Task.Yield();

            if(uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {uwr.error}");

            var result = _serializationOption.Deserialize<TResultType>(uwr.downloadHandler.text);
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError($"[{nameof(Get)}] failed: {ex.Message}");
            return default;
        }
    }
}
