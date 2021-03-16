using Mirror;
using TMPro;
using UnityEngine;

namespace TopDown
{
    public class Player : NetworkBehaviour
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
        [SyncVar(hook = nameof(OnNameUpdated))] string playerName;
        [SyncVar(hook = nameof(OnHealthUpdated))] float health;


        [SyncVar] bool isAlive;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            GameNetworkManager.singleton.AddPlayer(this);
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            CmdInitPlayer(GameNetworkManager.singleton.PlayerName);
        }

        [Command]
        void CmdInitPlayer(string pName)
        {
            playerName = pName;
            health = maxHealth;
            isAlive = true;
            UpdateUI();
        }

        void OnNameUpdated(string oldVal, string newVal)
        {
            UpdateUI();
        }

        void OnHealthUpdated(float oldVal, float newVal)
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            txt_playerName.text = playerName;
            healthBar.localScale = new Vector3(health / maxHealth, 1, 1);
        }

        void ShootBullet(Vector3 startPos, Vector3 bulletFwd)
        {
            GameObject go = Instantiate(bulletPrefab, startPos, Quaternion.identity);
            go.transform.forward = bulletFwd;
            go.GetComponent<Bullet>().ownerNetId = netId;
        }

        [Command]
        void CmdShoot()
        {
            ShootBullet(shootingPos.position, shootingPos.forward);
            RpcShoot(shootingPos.position, shootingPos.forward);
        }

        [ClientRpc]
        void RpcShoot(Vector3 startPos, Vector3 fwd)
        {
            if (!isServer)
            {
                ShootBullet(startPos, fwd);
            }
        }

        void Update()
        {
            if (isAlive && isLocalPlayer)
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
                    CmdShoot();
                }
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            GameNetworkManager.singleton.RemovePlayer(netId);
        }

        [Server]
        public void TakeDamage(float damage, uint killer)
        {
            if (isAlive)
            {
                health -= damage;
                if (health <= 0)
                {
                    isAlive = false;
                    RpcRip(killer);
                }
            }
        }

        [ClientRpc]
        public void RpcRip(uint killerId)
        {
            TopDownUIManager.Instance.ShowNotification($"[{playerName}] was killed by [{GameNetworkManager.singleton.NetworkPlayers[killerId].playerName}]");
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

}