using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PUN_TopDown
{
    public class PunUIManager : MonoBehaviourPunCallbacks
    {

        [SerializeField]
        TextMeshProUGUI txtPlayerList;
        [SerializeField]
        GameObject StartGameButton;
        [SerializeField] GameObject ColorPicker;

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

        public void UpdatePlayersList(string playersText)
        {
            txtPlayerList.text = playersText;
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
        }
        public void DisplayColorPicker()
        {
            ColorPicker.SetActive(true);
        }
    }
}