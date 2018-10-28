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
    private Rigidbody2D _myRigidbody2D;
    public LayerMask DeadEnemyLayer;

    // Use this for initialization
	void Start ()
	{
	    _myCircleCollider2D = GetComponent<CircleCollider2D>();
	    _myRigidbody2D = GetComponent<Rigidbody2D>();

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
        _myRigidbody2D.velocity = Vector2.zero;
        _myRigidbody2D.gravityScale = 1f;
        _myRigidbody2D.AddForce(Vector2.up * DeathJumpStrength);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.tag);
        if(col.CompareTag(DespawnZoneTag)) Destroy(gameObject);
    }
}
