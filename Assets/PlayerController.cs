using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D player;

    public float horizontal;
    public float vertical;
    public float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        player.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}