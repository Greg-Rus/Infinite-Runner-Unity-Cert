using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int HitPoints = 1;
    [SerializeField] private int _hpLeft;
    public string PlayerTag;
    public string DespawnZoneTag;
    public float DeathJumpStrength;
    private CircleCollider2D _myCircleCollider2D;
    protected Rigidbody2D MyRigidbody2D;
    public LayerMask DeadEnemyLayer;
    protected bool IsAlive = true;
    public Action<int> OnDeathCallback;

    // Use this for initialization
	void Start ()
	{
	    Initialize();
	}

    protected virtual void Initialize()
    {
        _myCircleCollider2D = GetComponent<CircleCollider2D>();
        MyRigidbody2D = GetComponent<Rigidbody2D>();
        _hpLeft = HitPoints;
    }

    // Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage()
    {
        _hpLeft--;
        if (_hpLeft <= 0)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        gameObject.layer = 10;
        MyRigidbody2D.velocity = Vector2.zero;
        MyRigidbody2D.gravityScale = 1f;
        MyRigidbody2D.AddForce(Vector2.up * DeathJumpStrength);
        IsAlive = false;

        OnDeathCallback(HitPoints);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag(DespawnZoneTag)) Destroy(gameObject);
    }
}
