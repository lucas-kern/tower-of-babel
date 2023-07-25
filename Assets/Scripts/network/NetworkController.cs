using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

//TODO ORGANIZE THESE FILES IN TO DIRECTORIES

// This class controls network managers to make appropriate requests to an API
// This class implements MonoBehavior so can be attached to a game object
public class NetworkController : MonoBehaviour
{

    public string url;
    private NetworkManager networkManager;
    private void Awake()
    {
        networkManager = new NetworkManager(new JsonSerializationOption());
    }

    // Gets JSON response from a URL
    public async void GetUser(string path)
    {
        var endpoint = url + path;
        var result = await networkManager.Get<User>(endpoint);
        // Process the result as needed
        if (result != null)
        {
            Debug.Log("GET request was successful!");
            // Handle the successful result
        }
        else
        {
            Debug.Log("GET request failed!");
            // Handle the failure or default result
        }
    }

    // TODO have this not return a user but return different info it will still post a user object then have registration and login methods to wrap this post which will return just success/failure for registration and will return a user for login
    public async Task<User> PostUser<T>(string path, T requestData)
    {
        var endpoint = url + path;
        var result = await networkManager.Post<User, T>(endpoint, requestData);
        // Process the result as needed
        if (result != null)
        {
            // Access the data from the response object of type T
            // The actual type of result will be based on the type argument T provided
            // by the caller of the method
            // For example, if T is User, then the result will be of type User
            // If T is some other type, then the result will be of that type
            Debug.Log("Response: " + result.ToString());
            return result;
            // Handle the successful result
        }
        else
        {
            Debug.Log("POST request failed!");
            // Handle the failure or default result
        }

        return null;
    }
}
