using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 30f;
    public Rigidbody2D rb;
    public float gravityScale = 1.0f;

    private float moveX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveX;
        rb.linearVelocity = velocity;
    }
}
