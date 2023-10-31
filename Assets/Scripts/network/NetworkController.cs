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
    private User userData;
    private void Awake()
    {
        networkManager = new NetworkManager(new JsonSerializationOption());
        userData = UserDataHolder.Instance.UserData; 
    }

    // Send a post request to the backend and retry if it fails also will refresh token if it fails with an authentication error
    public async Task<ApiResponse<TResultType>> PostWithRetry<TResultType, TRequestType>(string url, TRequestType requestData)
    {
        var token = userData.token;
        int maxRetries = 3;
        int currentRetry = 0;
        while (currentRetry < maxRetries)
        {
            try
            {
                var headers = new Dictionary<string, string>();
                // Add default headers here
                headers.Add("Authorization", token);
                var result = await networkManager.Post<TResultType, TRequestType>(url, requestData, headers);
                Debug.Log(typeof(TResultType).Name);
                if (result.meta.statusCode < 500 && result.meta.statusCode != 401)
                {
                    return result;
                }
                else if (result.meta.statusCode == 401)
                {
                    // Request failed, attempt to refresh the token and retry
                    bool refreshTokenSuccess = await RefreshAuthToken();

                    if (refreshTokenSuccess)
                    {
                        token = userData.token;
                        Debug.Log("Refresh token was a success");
                        // Retry the request with the updated token
                        result = await networkManager.Post<TResultType, TRequestType>(url, requestData, headers);
                        return result;
                    }
                    else
                    {
                        Debug.Log("Refreshing failed");
                        // Token refresh failed, return null or handle as needed
                        return default;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{nameof(PostWithRetry)}] failed: {ex.Message}");
                return null;
            }
            currentRetry++;
        }

        return default;
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
    //These user methods are strongly coded to the backend so if I chagne what is returned on backend these types also need to be updated is there something I can do to prevent that?

    public async Task<User> LoginUser(string path, User requestData)
    {
        var endpoint = url + path;
        var result = await networkManager.Post<User, User>(endpoint, requestData);
        
        // Process the result as needed
        if (result.meta.statusCode == 200)
        {
            // Access the data from the response object of type T
            // The actual type of result will be based on the type argument T provided
            // by the caller of the method
            // For example, if T is User, then the result will be of type User
            // If T is some other type, then the result will be of that type
            Debug.Log("Response: " + result.ToString());
            return result.data;
            // Handle the successful result
        }
        else
        {
            Debug.Log("POST request failed!");
            // Handle the failure or default result
        }

        return default;
    }

    public async Task<MetaData> RegisterUser(string path, User requestData)
    {
        var endpoint = url + path;
        var result = await networkManager.Post<string, User>(endpoint, requestData);
        
        // Process the result as needed
        if (result.meta.statusCode == 200)
        {
            // Access the data from the response object of type T
            // The actual type of result will be based on the type argument T provided
            // by the caller of the method
            // For example, if T is User, then the result will be of type User
            // If T is some other type, then the result will be of that type
            Debug.Log("Response: " + result.ToString());
            return result.meta;
            // Handle the successful result
        }
        else
        {
            Debug.Log("POST request failed!");
            // Handle the failure or default result
        }

        return default;
    }
    
    // Post an object to the specified path
    public async Task<(bool, TResultType)> PostObject<TResultType, TRequestType>(string path, TRequestType data)
    {
        var endpoint = url + path;
        var result = await PostWithRetry<TResultType, TRequestType>(endpoint, data);
        Debug.Log("In the Post object");
        Debug.Log(result);
        if (result.data != null)
        {
            Debug.Log("POST request was successful!");
            return (true, result.data); // Return true to indicate success
        }
        else
        {
            Debug.Log("POST request failed!");
            return (false, result.data); // Return false to indicate failure
        }
    }

    public async Task<Base> PlaceBuilding(Building data)
    {
        var path = "bases/place";
        var result = await PostObject<Base, Building>(path, data);
        Debug.Log("In the Place Building");
        Debug.Log(result.Item2);

        if (result.Item1 == true && result.Item2 != null)
        {
            Debug.Log("POST request was successful!");
            return result.Item2; // Return the base to indicate success
        }
        else
        {
            Debug.Log("POST request failed!");
            return null; // Return null to indicate failure
        }
    }

    // Refreshed the auth token using the refresh token 
    private async Task<bool> RefreshAuthToken()
    {
        var path = "/token";
        var endpoint = url + path;
        var refreshToken = userData.refresh_token;
        var userID = userData.user_id;


        try
        {
            // Create an instance of TokenRefreshRequestData and set its properties
            var headers = new Dictionary<string, string>();
            // Add default headers here
            headers.Add("refresh_token", refreshToken);
           
            var result = await networkManager.Get<TokenRefreshData>(endpoint, headers);

            if (result.data != null)
            {
                Debug.Log("Token Refresh request was successful!");
                userData.token = result.data.token;
                userData.refresh_token = result.data.refresh_token;
                return true; // Return true to indicate success
            }
            else
            {
                Debug.Log("Token Refresh request failed!");
                return false; // Return false to indicate failure
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{nameof(RefreshAuthToken)}] failed: {ex.Message}");
            return false;
        }
    }
}
