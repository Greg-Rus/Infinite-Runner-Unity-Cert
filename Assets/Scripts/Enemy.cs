using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int HitPoints = 1;
    public string PlayerTag;
    public string DespawnZoneTag;
    public float DeathJumpStrength;
    private CircleCollider2D _myCircleCollider2D;
    protected Rigidbody2D MyRigidbody2D;
    public LayerMask DeadEnemyLayer;
    protected bool IsAlive = true;

    // Use this for initialization
	void Start ()
	{
	    Initialize();
	}

    protected virtual void Initialize()
    {
        _myCircleCollider2D = GetComponent<CircleCollider2D>();
        MyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage()
    {
        HitPoints--;
        Debug.Log("Enemy damaged. HP: " +HitPoints);
        if (HitPoints <= 0)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        Debug.Log(gameObject.name + " died!");
        gameObject.layer = 10;
        MyRigidbody2D.velocity = Vector2.zero;
        MyRigidbody2D.gravityScale = 1f;
        MyRigidbody2D.AddForce(Vector2.up * DeathJumpStrength);
        IsAlive = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        if(col.CompareTag(DespawnZoneTag)) Destroy(gameObject);
    }
}
