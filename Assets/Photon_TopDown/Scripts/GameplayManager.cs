using Photon.Pun;
using UnityEngine;

namespace PUN_TopDown
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] Transform[] spawningPositions;
        [SerializeField] string playerPrefab;


        void Start()
        {
            Transform pTransform = spawningPositions[(PhotonNetwork.LocalPlayer.ActorNumber - 1) % spawningPositions.Length];
            PhotonNetwork.Instantiate(playerPrefab, pTransform.position, pTransform.rotation);
        }
    }

}