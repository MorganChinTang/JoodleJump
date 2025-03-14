using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;
    public GameObject playerPrefab;
    public GameObject platformPrefab;
    public float despawnDistance =70f;
    private float despawnThreshold;

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


    // Update is called once per frame
    void Update()
    {
        despawnThreshold = playerPrefab.transform.position.y - despawnDistance;
        if (platformPrefab.transform.position.y < despawnThreshold)
        {
            GameObject.Destroy(platformPrefab);
            GameManager.Instance.activePlatforms.Remove(platformPrefab);
        }
    }
}
