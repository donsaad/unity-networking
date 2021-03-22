using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PUN_TopDown
{
    public class PunUIManager : MonoBehaviourPun
    {

        [SerializeField]
        TextMeshProUGUI txtPlayerList;
        [SerializeField]
        GameObject StartGameButton;
        [SerializeField] GameObject ColorPicker;
        [SerializeField] Button readyButton;
        public static PunUIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            txtPlayerList.text = "";
        }
        [PunRPC]
        public void UpdatePlayersList()
        {
            txtPlayerList.text = GameNetworkManager.Instance.GetPlayersListAsString();
        }
        public void SetStartGameButton()
        {
            if (GameNetworkManager.Instance.IsHost())
            {
                StartGameButton.SetActive(true);
            }
        }

        public void OnClickReady()
        {
            if (GameNetworkManager.Instance.IsHost())
            {
                StartGameButton.GetComponent<Button>().interactable = true;
            }
            readyButton.interactable = false;
            readyButton.image.color = Color.green;
            //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            //hash.Add("IsReady", true);
            //photonView.Owner.SetCustomProperties(hash);
        }
        public void DisplayColorPicker()
        {
            ColorPicker.SetActive(true);
        }
    }
}
