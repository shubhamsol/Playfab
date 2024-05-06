using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PlayFabFriend: MonoBehaviour
    {
        List<FriendInfo> _friends = null;

        [SerializeField] private PlayerItem playerItem;

        [SerializeField] private TMP_InputField friendUsername;

        [SerializeField] private Transform parent;

        [SerializeField] private TextMeshProUGUI debug;
        public void GetFriends() {
            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), result => {
                _friends = result.Friends;
                DisplayFriends(_friends); // triggers your UI
            }, DisplayPlayFabError);
        }

        private void DisplayFriends(List<FriendInfo> friends)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
            foreach (var friend in _friends)
            {
                Instantiate(playerItem,parent).Init(friend.TitleDisplayName);
            }
        }

        private void DisplayPlayFabError(PlayFabError obj)
        {
            Debug.Log(obj.Error);
            debug.text = obj.Error + "";
        }
        
        public void AddFriend() {
            var request = new AddFriendRequest();
            request.FriendTitleDisplayName = friendUsername.text;
                
            
            // Execute request and update friends when we are done
            PlayFabClientAPI.AddFriend(request, result => {
                Debug.Log("Friend added successfully!");
                GetFriends();
            }, DisplayPlayFabError);
        }

        public void LogOut()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}