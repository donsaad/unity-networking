using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PUN_TopDown
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] GameObject connectionPanel;
        [SerializeField] GameObject lobbyPanel;
        [SerializeField] TMP_InputField if_roomName;

        // Start is called before the first frame update
        void Start()
        {
            GameNetworkManager.Instance.NM_OnConnected += OnConnectedToMaster;

            connectionPanel.SetActive(true);
            lobbyPanel.SetActive(false);
        }

        public void ConnectToMaster()
        {
            GameNetworkManager.Instance.ConnectToServer();
        }

        public void OnPlayerNameUpdated(string newName)
        {
            if (!string.IsNullOrEmpty(newName))
            {
                GameNetworkManager.Instance.SetPlayerName(newName); 
            }
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrEmpty(if_roomName.text))
            {
                GameNetworkManager.Instance.CreateRoom(if_roomName.text);
            }
        }

        public void JoinRoom()
        {
            if (!string.IsNullOrEmpty(if_roomName.text))
            {
                GameNetworkManager.Instance.JoinRoom(if_roomName.text);
            }
        }

        public void LoadGameplay()
        {
            GameNetworkManager.Instance.LoadScene(GameNetworkManager.SCENE_Gameplay);
        }

        void OnConnectedToMaster()
        {
            connectionPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }

        private void OnDestroy()
        {
            GameNetworkManager.Instance.NM_OnConnected -= OnConnectedToMaster;
        }
    }
}