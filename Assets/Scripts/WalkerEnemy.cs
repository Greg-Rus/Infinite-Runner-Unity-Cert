using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerEnemy : Enemy
{
    public float speed;

    public Vector2 direction = Vector2.right;

    public LayerMask groundLayer;
	// Use this for initialization
    void Start()
    {
        base.Initialize();
    }
	// Update is called once per frame
	void Update ()
	{
	    if (IsAlive)
	    {
	        Move();
	        DetectEdge();
        }
	}

    private void Move()
    {
        MyRigidbody2D.AddForce(this.direction*speed);
        if (MyRigidbody2D.velocity == Vector2.zero)
        {
            Debug.Log("Velocity turn");
            TurnAround();
        }
    }

    private void DetectEdge()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction + Vector2.down, 2f, groundLayer);
        Debug.DrawRay(transform.position, direction + Vector2.down * 2, Color.red);
        if (!hit)
        {
            Debug.Log("Turning!");
            TurnAround();
        }
        else
        {
            Debug.Log(hit.collider.tag);
        }


    }

    private void TurnAround()
    {
        
        direction *= -1;
        MyRigidbody2D.velocity = direction;
        MyRigidbody2D.angularVelocity = 0;
    }
}
