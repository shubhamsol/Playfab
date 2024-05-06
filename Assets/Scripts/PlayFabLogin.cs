using Helper;using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayFabLogin
{
    private static string _username;
    public static void LogIn(string username)
    {
        _username = username;
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "F5B02";
        }
        var request = new LoginWithCustomIDRequest { CustomId = username, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private static void OnLoginSuccess(LoginResult result)
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = _username };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,OnNameChaned,OnLoginFailure);
        Signals.Get<OnLoginSuccessSignal>().Dispatch();
        Debug.Log("Congratulations, you made your first successful API call! " + result.PlayFabId);
    }

    private static void OnNameChaned(UpdateUserTitleDisplayNameResult obj)
    {
        
    }

    private static void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}

public class OnLoginSuccessSignal:ASignal{}