using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;
    public GameObject playerPrefab;
    private float despawnDistance = 70f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.y <= 0f)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }
    }

    void Update()
    {
        if (playerPrefab != null && GameManager.Instance != null)
        {
            float playerY = playerPrefab.transform.position.y;
            
            // Check if this platform is too far below the player
            if (transform.position.y < playerY - despawnDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
