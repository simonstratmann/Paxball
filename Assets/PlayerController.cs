using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;

    public float horizontal;
    public float vertical;
    public float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;

    private Vector2 _currentInputVector;
    private Vector2 _smoothInputVelocity;
    [SerializeField] private float shootingPower = 20f;
    [SerializeField] private float smoothInputSpeed = 0.2f;
    private Collider2D _playerCollider;
    private GameObject _ball;
    private Collider2D _ballCollider;
    private Rigidbody2D _ballBody;

    void Start()
    {
        _playerCollider = playerBody.GetComponent<Collider2D>();
        
        _ball = GameObject.FindGameObjectWithTag("Ball");
        _ballCollider = _ball.GetComponent<Collider2D>();
        _ballBody = _ball.GetComponent<Rigidbody2D>();
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        var ballFieldMask = LayerMask.NameToLayer("BallField");
        
        if (other.gameObject.layer == ballFieldMask)
        {
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

        Vector2 input = new Vector2(horizontal, vertical);
        _currentInputVector = Vector2.SmoothDamp(_currentInputVector, input, ref _smoothInputVelocity, smoothInputSpeed);
        playerBody.velocity = new Vector2(_currentInputVector.x * runSpeed, _currentInputVector.y * runSpeed);
        playerBody.AddForce(new Vector2(1,0));

        if (_playerCollider.IsTouching(_ballCollider))
        {
            Vector2 v = _ballBody.gameObject.transform.position - transform.position;
            v /= v.magnitude;
            v = v * shootingPower;
            Debug.Log("Shooting with " + v);
            _ballBody.AddForce(v);
        }
        
        
    }
}