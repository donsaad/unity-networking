using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float damage;
        Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            Destroy(gameObject, 3);
        }

        void Update()
        {
            rb.velocity = transform.forward * speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.TakeDamage(damage, player.netId);
            }
            Destroy(gameObject);
        }
    }

}