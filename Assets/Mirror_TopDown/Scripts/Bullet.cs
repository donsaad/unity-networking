using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float speed;
        Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            Destroy(gameObject, 3);
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = transform.forward * speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Check for player and apply damage
            Destroy(gameObject);
        }
    }

}