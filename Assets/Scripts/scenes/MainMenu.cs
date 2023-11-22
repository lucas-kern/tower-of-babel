using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public string path;
    public NetworkController networkController;
    public TextMeshProUGUI errorMessageText; 

    private void OnEnable()
    {
        // Reset the error message when the menu is opened
        errorMessageText.text = "";
    }

    private void OnDisable()
    {
        // Reset the error message when the menu is opened
        errorMessageText.text = "";
    }


    public async void Login()
    {
        // Create a new user object
        User user = new User();
        user.email = emailInput.text;
        user.password = passwordInput.text;

        // Make the login API call and wait for the result
        User userData = await networkController.LoginUser(path, user);

        // Process the result as needed
        if (userData != null)
        {
            // Handle successful login
            Debug.Log("Login successful!");

            // Store the user data in the UserDataHolder singleton
            UserDataHolder.Instance.UserData = userData;

            // Load the next scene in the scene manager
            // You can also use SceneManager.LoadScene(sceneName) to load scenes by name
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // Handle login failure
            Debug.Log("Login failed!");
            errorMessageText.text = "Login failed. Please check your credentials and try again.";
        }
    }

    public async void Register()
    {
                // Create a new user object
        User user = new User();
        user.email = emailInput.text;
        user.password = passwordInput.text;
        user.firstName = firstNameInput.text;
        user.lastName = lastNameInput.text;

        // Make the login API call and wait for the result
        // if a user is returned let's just log them in 
        MetaData apiData  = await networkController.RegisterUser(path, user);

        // Process the result as needed
        if (apiData != null && apiData.statusCode == 200)
        {
            // Handle successful login
            Debug.Log("Registration successful!");
            errorMessageText.text = "Registration Successful. Please continue to login.";
        }
        else
        {
            // Handle login failure
            // show a try again message if user isn't registered
            Debug.Log("Registration failed!");
            errorMessageText.text = "Registration failed. Please try again later.";
        }
        // Load the next scene in the scene manager
        // Can also use this method to call scenes by name`
    }
}
