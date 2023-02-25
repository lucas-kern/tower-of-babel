using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Login()
    {
        // Load the next scene in the scene manager
        // Can also use this method to call scenes by name
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Register()
    {
        // Load the next scene in the scene manager
        // Can also use this method to call scenes by name
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
