using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 30f;
    public Rigidbody2D rb;
    public float gravityScale = 1.0f;
    public UnityEngine.UI.Button LeftButton;
    public UnityEngine.UI.Button RightButton;

    private float moveX;

    public static PlayerController Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        if (LeftButton != null)
        {
            var leftTrigger = LeftButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            var leftEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
            leftEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
            leftEntry.callback.AddListener((eventData) => { moveX = -moveSpeed; UpdateVelocity(); });
            leftTrigger.triggers.Add(leftEntry);

            var leftExit = new UnityEngine.EventSystems.EventTrigger.Entry();
            leftExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
            leftExit.callback.AddListener((eventData) => { moveX = 0; });
            leftTrigger.triggers.Add(leftExit);
        }

        if (RightButton != null)
        {
            var rightTrigger = RightButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            var rightEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
            rightEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
            rightEntry.callback.AddListener((eventData) => { moveX = moveSpeed; UpdateVelocity(); });
            rightTrigger.triggers.Add(rightEntry);

            var rightExit = new UnityEngine.EventSystems.EventTrigger.Entry();
            rightExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
            rightExit.callback.AddListener((eventData) => { moveX = 0; });
            rightTrigger.triggers.Add(rightExit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gamepause)
        {
            moveX = 0;
            return;
        }

        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float playerX = transform.position.x;

        if (playerX < -screenWidth)
        {
            transform.position = new Vector3(screenWidth, transform.position.y, transform.position.z);
        }
        else if (playerX > screenWidth)
        {
            transform.position = new Vector3(-screenWidth, transform.position.y, transform.position.z);
        }
    }

    void UpdateVelocity()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveX;
        rb.linearVelocity = velocity;
    }

    private void FixedUpdate()
    {
        // Removed the code from here
    }
}
