using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    static GameplayUIManager instance;
    [SerializeField] TextMeshProUGUI txt_playerName;
    [SerializeField] TextMeshProUGUI txt_oponentName;
    [SerializeField] TextMeshProUGUI txt_msg;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject playerActions;

    //Properties
    public static GameplayUIManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetName(string name, bool isMyPlayer)
    {
        if (isMyPlayer)
        {
            txt_playerName.text = name + ":";
        }
        else
        {
            txt_oponentName.text = name + ":";
        }
    }

    public void SetMove(int playerMove)
    {
        GameNetworkManager.singleton.GetLocalPlayer().CmdSetMove((RPSType)playerMove);
    }
    public void Replay()
    {
        GameNetworkManager.singleton.GetLocalPlayer().CmdSetReadyForReplay();
    }
    public void SendMessage(string msg)
    {
        gameOver.SetActive(true);
        playerActions.SetActive(false);
        txt_msg.SetText(msg);
        SetMove(0);
    }
    public void RestartGame()
    {
        if (GameNetworkManager.singleton.CheckIfAllReady())
        {
            gameOver.SetActive(false);
            playerActions.SetActive(true);
        }
    }
}
