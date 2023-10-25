using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

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
    public async Task<ApiResponse<TResultType>> Get<TResultType>(string url, Dictionary<string, string> headers = null)
    {
        try
        {
            using var uwr = UnityWebRequest.Get(url);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    uwr.SetRequestHeader(header.Key, header.Value);
                }
            }

            uwr.SetRequestHeader("Content-Type", _serializationOption.ContentType);
 
            var operation = uwr.SendWebRequest();

            while(!operation.isDone)
                await Task.Yield();

            if(uwr.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {uwr.error}");

            var result = _serializationOption.Deserialize<ApiResponse<TResultType>>(uwr.downloadHandler.text);
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError($"[{nameof(Get)}] failed: {ex.Message}");
            return default;
        }
    }

    // Make a POST request to an API
    public async Task<ApiResponse<TResultType>> Post<TResultType, TRequestType>(string url, TRequestType requestData, Dictionary<string, string> headers = null)
    {
        try
        {
            var requestBody = _serializationOption.Serialize(requestData);
            using var uwr = UnityWebRequest.Post(url, requestBody);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    uwr.SetRequestHeader(header.Key, header.Value);
                }
            }

            Debug.Log(requestBody);
            uwr.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            var operation = uwr.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            ApiResponse<TResultType> response = new ApiResponse<TResultType>
            {
                meta = new MetaData
                {
                    statusCode = (int)operation.webRequest.responseCode, // Set the status code from the response
                },
                data = default(TResultType) // Set data to null
            };

            if (operation.webRequest.responseCode == 200 && uwr.result == UnityWebRequest.Result.Success)
            {
                response.data = _serializationOption.Deserialize<ApiResponse<TResultType>>(uwr.downloadHandler.text).data;
                Debug.Log("we are in the result: " + response);
            }
            else
            {
                Debug.LogError($"Request failed with response code: {operation.webRequest.responseCode}");
            }
            Debug.Log("we are in the result: " + response);
            return response;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(Post)}] failed: {ex.Message}");
            return new ApiResponse<TResultType>
        {
            meta = new MetaData
            {
                statusCode = 500, // Set a default status code (e.g., 500 for Internal Server Error)
            },
            data = default(TResultType)
        };
        }
    }
}
