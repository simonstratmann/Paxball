using System;
using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;

    public float horizontal;
    public float vertical;
    public float moveLimiter = 0.7f;
    [SerializeField] private float shootingPower = 300f;
    [SerializeField] private float smoothInputSpeed = 0.3f;
    [SerializeField] private int framesBetweenShots = 2;
    [SerializeField] private float runSpeed = 7.0f;
    [SerializeField] private float runSpeedWhileShooting = 4.0f;

    public FloatEvent shootingPowerEvent; 

    public float ShootingPower
    {
        get => shootingPower;
        set => shootingPower = value;
    }

    public float SmoothInputSpeed
    {
        get => smoothInputSpeed;
        set => smoothInputSpeed = value;
    }

    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    public float RunSpeedWhileShooting
    {
        get => runSpeedWhileShooting;
        set => runSpeedWhileShooting = value;
    }


    private Vector2 _currentInputVector;
    private Vector2 _smoothInputVelocity;
    private Collider2D _playerCollider;
    private GameObject _player;
    private GameObject _ball;
    private Collider2D _ballCollider;
    private Rigidbody2D _ballBody;
    private int _lastShotFrame;
    private Boolean _mayShoot;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _playerCollider = playerBody.GetComponent<Collider2D>();

        _ball = GameObject.FindGameObjectWithTag("Ball");
        _ballCollider = _ball.GetComponent<Collider2D>();
        _ballBody = _ball.GetComponent<Rigidbody2D>();
        _player = playerBody.GetComponentInParent<GameObject>();
        _spriteRenderer = _player.GetComponentInParent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var ballFieldMask = LayerMask.NameToLayer("BallField");

        if (other.gameObject.layer == ballFieldMask)
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), other.collider);
        }
    }
    
    public float GetShootingPower()
    {
        return shootingPower;
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
        _currentInputVector =
            Vector2.SmoothDamp(_currentInputVector, input, ref _smoothInputVelocity, smoothInputSpeed);
        var isShooting = IsShooting();
        float actualRunSpeed = isShooting ? runSpeedWhileShooting : runSpeed;
        playerBody.velocity =
            new Vector2(_currentInputVector.x * actualRunSpeed, _currentInputVector.y * actualRunSpeed);

        var isTouching = _playerCollider.IsTouching(_ballCollider);
        if (isShooting)
        {
            playerBody.GetComponentInParent<SpriteRenderer>().color = _mayShoot ? Color.white : Color.black;
            if (isTouching)
            {
                if (Time.frameCount - _lastShotFrame <= framesBetweenShots)
                {
                    Debug.Log("Not yet shooting");
                    return;
                }

                if (!_mayShoot)
                {
                    Debug.Log("May not yet shoot");
                    return;
                }

                _lastShotFrame = Time.frameCount;
                _mayShoot = false;

                Vector2 v = _ballBody.gameObject.transform.position - transform.position;
                v /= v.magnitude;
                v = v * shootingPower;
                Debug.Log("Shooting with " + v);
                _ballBody.AddForce(v);
            }
        }
        else
        {
            _mayShoot = true;
            playerBody.GetComponentInParent<SpriteRenderer>().color = Color.black;
        }
    }

    private static bool IsShooting()
    {
        return Input.GetKey(KeyCode.Space);
    }
}