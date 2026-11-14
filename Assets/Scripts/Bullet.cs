using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed = 20f; // Speed of the bullet

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f); // Destroy the bullet after 3 seconds
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * speed; // Move the bullet forward at a speed of 20 units per second
    }
}
