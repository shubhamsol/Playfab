using System;
using Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LogInWindow : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private PlayFabFriend friendsWindow;
        private void Start()
        {
            Signals.Get<OnLoginSuccessSignal>().AddListener(OnLoggedIn);
        }

        private void OnLoggedIn()
        {
            gameObject.SetActive(false);
            friendsWindow.gameObject.SetActive(true);
            friendsWindow.GetFriends();
        }

        public void LogIn()
        {
            if(usernameInputField.text.Length>2)
                PlayFabLogin.LogIn(usernameInputField.text.ToLower());
        }

        private void OnDisable()
        {
            Signals.Get<OnLoginSuccessSignal>().RemoveListener(OnLoggedIn);
        }
    }
}