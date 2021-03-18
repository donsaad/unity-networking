using TMPro;
using UnityEngine;

namespace RPS
{
	public class GameplayUIManager : MonoBehaviour
	{
	    static GameplayUIManager instance;
	    [SerializeField] TextMeshProUGUI txt_playerName;
	    [SerializeField] TextMeshProUGUI txt_playerMove;
	    [SerializeField] TextMeshProUGUI txt_oponentName;
	    [SerializeField] TextMeshProUGUI txt_oponentMove;
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
	    public void UpdateUIPanels(RPSType playerOneMove, RPSType playerTwoMove, string msg, bool hasAuthority)
	    {
	        gameOver.SetActive(true);
	        playerActions.SetActive(false);
	        txt_msg.SetText(msg);
	        if (hasAuthority)
	        {
	            txt_playerMove.text = playerOneMove.ToString();
	            txt_oponentMove.text = playerTwoMove.ToString();
	            Debug.Log("from if");
	        }
	        else
	        {
	            Debug.Log("else");
	            txt_playerMove.text = playerTwoMove.ToString();
	            txt_oponentMove.text = playerOneMove.ToString();
	        }
	    }
	    public void RestartGame()
	    {
	        if (GameNetworkManager.singleton.CheckIfAllReady())
	        {
	            gameOver.SetActive(false);
	            playerActions.SetActive(true);
	            txt_oponentMove.text = "***";
	            txt_playerMove.text = "***";
	        }
	    }
	}
}
