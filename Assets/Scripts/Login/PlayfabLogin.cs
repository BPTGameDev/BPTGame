using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using TMPro;

public class PlayfabLogin : MonoBehaviour
{
    [SerializeField] private string email; // username = email
    [SerializeField] private string walletaddress; // not relevant
    [SerializeField] private string password;
    [SerializeField] private string displayname;

    public static bool loggedIn;

    public TMP_InputField inputID;

    #region Unity Methods
    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "374E0";
        }
    }
    #endregion

    #region Private Methods

    private bool IsValidDisplayname()
    {
        bool isValid = false;
        if (displayname.Length >= 3 && displayname.Length <= 24)
        {
            isValid = true;
        }
        return isValid;
    }
    private bool IsValidPassword()
    {
        bool isValid = false;
        if (password.Length >= 6 && password.Length <= 24)
        {
            isValid = true;
        }
        return isValid;
    }

    /*private void LoginWithCustomId()
    {
        Debug.Log($"Login to Playfab as {username}");
        var request = new LoginWithCustomIDRequest { CustomId = username, CreateAccount = true };
        var emailAddress = walletaddress;
        Debug.Log("email is " + emailAddress);
        // AddOrUpdateContactEmail(emailAddress);

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomIdSuccess, OnFailure);
        // AddContactEmailToPlayer();
        // Debug.Log("added email");
    }*/

    /*private void UpdateDisplayName(){
        Debug.Log($"Updating Playfab account's display name to: {displayname}");
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayname};
        // callback for when displayname update succeeds, or results in error
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnFailure);
    }*/

    #endregion

    #region Public Methods

    // PLAYING WITH EMAIL

    public void LoginWithEmail()
    {
        // 2
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginEmailSuccess, OnLoginEmailFailure);
    }

    //////////////////////

    public void SetDisplayName(string name)
    {
        displayname = name;
        PlayerPrefs.SetString("DISPLAYNAME", displayname);
    }

    public void SetEmail(string address)
    {
        email = address;
        PlayerPrefs.SetString("EMAIL", email);
    }

    public void SetPassword(string pw)
    {
        password = pw;
        PlayerPrefs.SetString("PASSWORD", password);
    }

    // public void SetEmail(string em)
    // {
    //     email = em;
    //     PlayerPrefs.SetString("EMAIL", email);
    //     var request = new AddOrUpdateContactEmail{ }
    //     PlayFabClientAPI.AddOrUpdateContactEmail(request, email, )
    // }

    public void SetWallet(string address)
    {
        walletaddress = address;
        PlayerPrefs.SetString("WALLET", walletaddress);
    }

    public void SaveEmail() { 
        var request = new UpdateUserDataRequest {
            Data = new Dictionary<string,string> { {"email",inputID.text.ToString()}}
        };
        PlayFabClientAPI.UpdateUserData(request, OnIDUpdateSuccess, OnError);
        Debug.Log("added email");
    }

    public void OnError(PlayFabError error)
    {
        // messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    public void OnIDUpdateSuccess(UpdateUserDataResult result) {
        Debug.Log("Success ID");
        SceneController.LoadScene("Menu");
    }
    public void AddContactEmailToPlayer()
    {
        var loginReq = new LoginWithCustomIDRequest
        {
            CustomId = "374E0", // replace with your own Custom ID
            CreateAccount = true // otherwise this will create an account with that ID
        };

        // var emailAddress = "jonathanyin66@gmail.com"; 
        var emailAddress = walletaddress;
        Debug.Log("email is " + emailAddress);

        var request = new LoginWithCustomIDRequest { CustomId = displayname, CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomIdSuccess =>
        {
            Debug.Log("Successfully logged in player with PlayFabId");
            AddOrUpdateContactEmail(emailAddress);//Removed the parameter -- "PlayFabId"
        }, FailureCallback);
    }


    public void AddOrUpdateContactEmail(string emailAddress)//Removed the parameter -- "PlayFabId"
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            //[Remove it]PlayFabId = playFabId,
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            Debug.Log("The player's account has been updated with a contact email");
        }, FailureCallback);
    }

    public void UpdateDisplayName()
    {
        if (IsValidDisplayname())
        {
            Debug.Log($"Updating Playfab account's display name to: {displayname}");
            var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayname };
            // callback for when displayname update succeeds, or results in error
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnFailure);
        }
        else
        {
            Debug.Log("Display name is invalid");
        }
    }

    public void Login()
    {
        // 1
        LoginWithEmail();
    }
    #endregion

    #region Playfab Callbacks

    // PLAYING WITH EMAIL
    //////////////////////
    private void OnLoginEmailSuccess(LoginResult result)
    {
        // 3
        Debug.Log($"Logged in successfully using {email}");
        SceneController.LoadScene("Menu");
        loggedIn = true;
    }

    private void OnLoginEmailFailure(PlayFabError error)
    {
        // 4
        Debug.Log($"Failed to log in using {email}");
        Debug.Log("Registering...");
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            DisplayName = email.Substring(0, email.IndexOf("@")),
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterRequest, OnRegisterFailure);
    }

    private void OnContactEmailSuccess(AddOrUpdateContactEmailResult result)
    {
        Debug.Log($"Updated contact email to {email}");
    }

    private void OnContactEmailFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnRegisterRequest(RegisterPlayFabUserResult result)
    {
        // 5
        Debug.Log($"Registering {email}");
        var updateRequest = new AddOrUpdateContactEmailRequest { EmailAddress = email };
        PlayFabClientAPI.AddOrUpdateContactEmail(updateRequest, OnContactEmailSuccess, OnContactEmailFailure);
        SceneController.LoadScene("Menu");
        loggedIn = true;
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        // 6
        Debug.Log(error.GenerateErrorReport());
    }

    //////////////////////////////////////////////

    private void OnLoginCustomIdSuccess(LoginResult result)
    {
        Debug.Log($"You have logged into Playfab using custom id {displayname}");
        UpdateDisplayName();
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request {error.GenerateErrorReport()}");
    }

    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result){
        Debug.Log($"You have updated the display name of the Playfab account!");
        SceneController.LoadScene("Menu");
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion
}
