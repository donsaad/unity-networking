using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PUN_TopDown
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] Transform[] spawningPositions;
        [SerializeField] string playerPrefab;

        // Start is called before the first frame update
        void Start()
        {
            Transform pTransform = spawningPositions[(PhotonNetwork.LocalPlayer.ActorNumber - 1) % spawningPositions.Length];
            PhotonNetwork.Instantiate(playerPrefab, pTransform.position, pTransform.rotation);
        }
    }

}