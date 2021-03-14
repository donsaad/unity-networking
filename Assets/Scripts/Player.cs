using Mirror;

public enum RPSType
{
    None = 0,
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetPlayerName))] public string playerName;
    [SyncVar] public RPSType playerMove;
    [SyncVar(hook = nameof(RestartGameIfPossible))] public bool IsReadyForReplay = false;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        CmdUpdatePlayerName(GameNetworkManager.singleton.PlayerName);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerMove = RPSType.None;
    }

    private void Start()
    {
        GameNetworkManager.singleton.AddPlayer(this);
    }

    void SetPlayerName(string oldName, string newName)
    {
        GameplayUIManager.Instance.SetName(newName, hasAuthority);
    }

    [Command]
    void CmdUpdatePlayerName(string name)
    {
        playerName = name;
    }

    [Command]
    public void CmdSetMove(RPSType move)
    {
        playerMove = move;
        GameNetworkManager.singleton.CalculateResult();
    }

    [Command]
    public void CmdSetReadyForReplay()
    {
        IsReadyForReplay = true;
    }

    [Server]
    public void SetNOTReadyForReplay()
    {
        IsReadyForReplay = false;
    }

    [TargetRpc]
    public void RpcUpdateUI(RPSType playerOneMove, RPSType playerTwoMove, string msg, bool authFlag)
    {
        GameplayUIManager.Instance.UpdateUIPanels(playerOneMove, playerTwoMove, msg, authFlag);
    }


    public void RestartGameIfPossible(bool oldBool, bool newBool)
    {
        GameplayUIManager.Instance.RestartGame();
    }
}
