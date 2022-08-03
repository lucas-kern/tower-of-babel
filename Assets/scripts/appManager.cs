using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Linq;

public class appManager : MonoBehaviour
{
    // API url
    public string url;

    public towerBase tower;
    // resulting JSON from an API request
    // public JSONNode jsonResult;

    // instance
    public static appManager instance;
    void Awake ()
    {
        // set the instance to be this script
        instance = this;
    }

    // sends an API request - returns a JSON file
    IEnumerator GetData(string location)
    {
        // create the web request and download handler
        UnityWebRequest webReq = new UnityWebRequest();
        webReq.downloadHandler = new DownloadHandlerBuffer();
        // build the url and query
        webReq.url = string.Format("{0}{1}", url, location);

        // send the web request and wait for a returning result
        yield return webReq.SendWebRequest();

        // convert the byte array and wait for a returning result
        string rawJson = Encoding.Default.GetString(webReq.downloadHandler.data);
        Debug.Log(rawJson);
        tower = towerBase.CreateFromJSON(rawJson);
        // parse the raw string into a json result we can easily read
        // jsonResult = JSON.Parse(rawJson);
        Debug.Log(towerBase.toString(tower));
        // display the results on screen
    }
}
