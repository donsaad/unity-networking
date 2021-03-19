using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PUN_TopDown
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] GameObject connectionPanel;
        [SerializeField] GameObject lobbyPanel;
        [SerializeField] TextMeshProUGUI txtPlayerList;
        [SerializeField] TMP_InputField if_roomName;
        [SerializeField] List<Button> colorButtons;
        [SerializeField] Button readyButton;


        void Start()
        {
            GameNetworkManager.Instance.NM_OnConnected += OnConnectedToMaster;
            connectionPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            txtPlayerList.enabled = false;
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
            if (GameNetworkManager.Instance.GetCurrentPlayersCount() >= 2)
            {
                GameNetworkManager.Instance.LoadScene(GameNetworkManager.SCENE_Gameplay);
            }
            else
            {
                GameNetworkManager.Instance.LogMessage("Cant start game, at least 2 players needed");
            }
        }

        void OnConnectedToMaster()
        {
            connectionPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            txtPlayerList.enabled = true;
        }

        private void OnDestroy()
        {
            GameNetworkManager.Instance.NM_OnConnected -= OnConnectedToMaster;
        }

        public void OnColorSelected(int buttonIndex)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != buttonIndex)
                {
                    colorButtons[i].interactable = false;
                }
                else
                {
                    //TODO: GameNetworkManager.Instance.SetPlayerColor()
                }
            }
            readyButton.interactable = true;
        }

    }
}