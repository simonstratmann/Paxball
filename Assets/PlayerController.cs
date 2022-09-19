using System;
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
        LayerMask ballFieldLayerMask = LayerMask.GetMask("BallField");
        
        LayerMask playerLayerMask = LayerMask.GetMask("PlayerField");
        Physics2D.IgnoreLayerCollision(ballFieldLayerMask, playerLayerMask);
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        
        var ballFieldMask = LayerMask.NameToLayer("BallField");
        
        Debug.Log("Ball field mask: " + ballFieldMask);
        Debug.Log("This layer mask: " + gameObject.layer);
        Debug.Log("Other layer mask: " + other.gameObject.layer);
        if (other.gameObject.layer == ballFieldMask)
        {
            Debug.Log("Ignoring player collision with " + other);
            Debug.Log(other.collider);
            Debug.Log(other.otherCollider);
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), other.collider);
        }
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