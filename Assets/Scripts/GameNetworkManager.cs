using System.Collections.Generic;
using System.Linq;
using Mirror;

namespace RPS
{
    public class GameNetworkManager : NetworkManager
    {
        Dictionary<uint, Player> networkPlayers;

        public static GameNetworkManager singleton => NetworkManager.singleton as GameNetworkManager;
        public bool IsClient { get; private set; }
        public bool IsServer { get; private set; }
        public string PlayerName { get; set; }
        public Dictionary<uint, Player> NetworkPlayers => networkPlayers;

        private void Start()
        {
            networkPlayers = new Dictionary<uint, Player>();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            IsServer = true;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            IsClient = true;
        }

        public void AddPlayer(Player player)
        {
            if (player != null && !networkPlayers.ContainsKey(player.netId))
            {
                networkPlayers.Add(player.netId, player);
            }
        }

        public Player GetLocalPlayer()
        {
            Player retValue = null;
            foreach (var player in networkPlayers)
            {
                if (player.Value.hasAuthority)
                {
                    retValue = player.Value;
                    break;
                }
            }
            return retValue;
        }

        [Server]
        public void CalculateResult()
        {
            //Check the moves for each player
            Player playerOne = networkPlayers.First().Value;
            Player playerTwo = networkPlayers.Last().Value;
            string msg = "";

            if (playerOne.playerMove == RPSType.None || playerTwo.playerMove == RPSType.None)
            {
                return;
            }
            else if (playerOne.playerMove == playerTwo.playerMove)
            {
                msg = "Tie";
            }
            else if (playerOne.playerMove == RPSType.Rock && playerTwo.playerMove == RPSType.Paper)
            {
                msg = $"{playerTwo.playerName} wins";
            }
            else if (playerOne.playerMove == RPSType.Rock && playerTwo.playerMove == RPSType.Scissors)
            {
                msg = $"{playerOne.playerName} wins";
            }
            else if (playerOne.playerMove == RPSType.Paper && playerTwo.playerMove == RPSType.Rock)
            {
                msg = $"{playerOne.playerName} wins";
            }
            else if (playerOne.playerMove == RPSType.Paper && playerTwo.playerMove == RPSType.Scissors)
            {
                msg = $"{playerTwo.playerName} wins";
            }
            else if (playerOne.playerMove == RPSType.Scissors && playerTwo.playerMove == RPSType.Rock)
            {
                msg = $"{playerTwo.playerName} wins";
            }
            else if (playerOne.playerMove == RPSType.Scissors && playerTwo.playerMove == RPSType.Paper)
            {
                msg = $"{playerOne.playerName} wins";
            }
            //Send win and lose messages to the players then show players' moves
            if (msg.Length != 0)
            {
                foreach (var item in networkPlayers)
                {
                    item.Value.RpcUpdateUI(playerOne.playerMove, playerTwo.playerMove, msg, item.Value.hasAuthority);
                    item.Value.SetNOTReadyForReplay();
                }
                foreach (var item in networkPlayers)
                {
                    item.Value.playerMove = RPSType.None;
                }
                msg = "";
            }
        }

        public bool CheckIfAllReady()
        {
            foreach (var item in networkPlayers)
            {
                if (!item.Value.IsReadyForReplay)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
