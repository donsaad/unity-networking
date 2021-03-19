using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace PUN_TopDown
{
    public class GameNetworkManager : MonoBehaviourPunCallbacks
    {
        //Scenes
        public const string SCENE_MainMenu = "MainMenu";
        public const string SCENE_Gameplay = "Gameplay";

        static GameNetworkManager instance;
        private string gameVersion = "v0.1";

        [SerializeField] TextMeshProUGUI txt_log;
        event Action NM_onConnected;

        //Properties
        public static GameNetworkManager Instance => instance;
        public event Action NM_OnConnected { add => NM_onConnected += value; remove => NM_onConnected -= value; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ConnectToServer()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            // TODO: check for available rooms
            /*if (PhotonNetwork.CountOfRooms > 0)
            {
            }*/
        }

        public void SetPlayerName(string pName)
        {
            PhotonNetwork.NickName = pName;
        }

        public void SetPlayerColor(string pName)
        {
           // PhotonNetwork.SetPlayerCustomProperties();
        }


        public void CreateRoom(string roomName)
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void LoadScene(string sceneName)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(sceneName);
            }
            else
            {
                LogMessage("[Error] LoadScene should only be called from the master client");
            }
        }

        #region Callbacks

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            LogMessage("Connected to game server.");
            NM_onConnected?.Invoke();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            LogMessage("Room Created: " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnJoinedRoom()
        {
            LogMessage("Joined Room: " + PhotonNetwork.CurrentRoom.Name + " with " + PhotonNetwork.CurrentRoom.PlayerCount + " players");
            PunUIManager.Instance.SetStartGameButton();
            PunUIManager.Instance.DisplayColorPicker();
            PunUIManager.Instance.photonView.RPC(nameof(PunUIManager.UpdatePlayersList), RpcTarget.All);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            LogMessage(newPlayer.NickName + " entered the room");
            PunUIManager.Instance.photonView.RPC(nameof(PunUIManager.UpdatePlayersList), RpcTarget.All);
        }

        #endregion

        public void LogMessage(string msg)
        {
            txt_log.text = msg + '\n' + txt_log.text;
        }
        public void UpdatePlayersListUI()
        {
            string res = "Players in Room:\n";
            foreach (var player in PhotonNetwork.PlayerList)
            {
                res += player.NickName + "\n";
            }
        }

        public string GetPlayersListAsString()
        {
            string res = "Players in Room:\n";
            foreach (var player in PhotonNetwork.PlayerList)
            {
                res += player.NickName + "\n";
            }
            return res;
        }

        public bool IsHost()
        {
            return PhotonNetwork.IsMasterClient;
        }
        public byte GetCurrentPlayersCount()
        {
            return PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }
}