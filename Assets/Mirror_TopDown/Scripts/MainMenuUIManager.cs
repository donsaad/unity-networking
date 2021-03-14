using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TopDown
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] TMP_InputField if_playerName;
        [SerializeField] Button[] connectionButtons;

        // Start is called before the first frame update
        void Start()
        {
            ValidateName();
        }

        public void ValidateName()
        {
            if (string.IsNullOrEmpty(if_playerName.text))
            {
                for (int i = 0; i < connectionButtons.Length; i++)
                {
                    connectionButtons[i].interactable = false;
                }
            }
            else
            {
                GameNetworkManager.singleton.PlayerName = if_playerName.text;
                for (int i = 0; i < connectionButtons.Length; i++)
                {
                    connectionButtons[i].interactable = true;
                }
            }
        }
    }

}