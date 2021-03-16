using System.Collections.Generic;
using Mirror;

namespace MirrorTopDown
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

        public void RemovePlayer(uint netId)
        {
            if (networkPlayers.ContainsKey(netId))
            {
                networkPlayers.Remove(netId);
            }
        }
    }

}