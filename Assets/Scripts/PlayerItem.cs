using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;

        public void Init(string customID)
        {
            playerName.text = customID;
        }
    }
}