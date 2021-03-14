using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TopDown
{
    public class TopDownUIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txt_notification;
        [SerializeField] GameObject notifPanel;
        public static TopDownUIManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        IEnumerator ShowTempNotification(string message)
        {
            notifPanel.SetActive(true);
            txt_notification.SetText(message);
            yield return new WaitForSeconds(3);
            notifPanel.SetActive(false);
        }
        public void ShowNotification(string message)
        {
            StartCoroutine(ShowTempNotification(message));
        }
    }
}
