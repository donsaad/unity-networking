using UnityEngine;

namespace PUN_TopDown
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float speed;
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
            //Check for player and apply damage
            Destroy(gameObject);
        }
    }

}