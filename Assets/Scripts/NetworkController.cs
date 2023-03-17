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
    // Gets JSON from a test URL
    // TODO update this to take in the URL from Unity and test with the backend
    public async void Get()
    {
        string url = "http://jsonplaceholder.typicode.com/todos/1";

        var networkManager = new NetworkManager(new JsonSerializationOption());
        var result = await networkManager.Get<User>(url);
    }
}
