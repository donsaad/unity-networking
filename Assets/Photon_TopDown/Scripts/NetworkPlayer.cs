using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

namespace PUN_TopDown
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        [Header("Movement")]
        [SerializeField] float movementSpeed;
        [SerializeField] float cannonRotSpeed;
        [SerializeField] Transform cannonPivot;
        Rigidbody rb;

        [Header("Shooting")]
        [SerializeField] Transform shootingPos;
        [SerializeField] GameObject bulletPrefab;

        [Header("UI")]
        [SerializeField] TextMeshProUGUI txt_playerName;
        [SerializeField] Transform healthBar;
        [SerializeField] float maxHealth;
        string playerName;
        float health;


        void Start()
        {
            txt_playerName.text = photonView.Owner.NickName;
            if (photonView.IsMine)
            {
                rb = GetComponent<Rigidbody>();
                health = maxHealth;

                //UpdateUI();
            }
        }

        void UpdateUI()
        {
            healthBar.localScale = new Vector3(health / maxHealth, 1, 1);
        }

        [PunRPC]
        public void Shoot()
        {
            Instantiate(bulletPrefab, shootingPos.position, shootingPos.rotation);
        }


        void Update()
        {
            if (photonView.IsMine)
            {
                float xMov = Input.GetAxis("Horizontal");
                float yMov = Input.GetAxis("Vertical");
                rb.MovePosition(new Vector3(rb.position.x + xMov * movementSpeed * Time.deltaTime, rb.position.y, rb.position.z + yMov * movementSpeed * Time.deltaTime));

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    cannonPivot.RotateAround(transform.up, cannonRotSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    cannonPivot.RotateAround(transform.up, -cannonRotSpeed * Time.deltaTime);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC(nameof(Shoot), RpcTarget.All);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(health);
            }
            else
            {
                health = (float)stream.ReceiveNext();
            }

            UpdateUI();
        }
    }

}