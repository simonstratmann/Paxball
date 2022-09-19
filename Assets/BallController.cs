using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    
    public Rigidbody2D ballBody;
    
    [SerializeField] private float bounciness = 6f;
    private GameObject _levelBorder;
    private Collider2D _levelCollider;

    // Start is called before the first frame update
    void Start()
    {
        _levelBorder = GameObject.FindGameObjectWithTag("LevelBorder");
        _levelCollider = _levelBorder.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag.Equals("LevelBorder"))
        {
            ballBody.AddForce(other.contacts[0].normal * bounciness);
        }
        
    }
}
