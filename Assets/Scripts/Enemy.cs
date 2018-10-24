using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VR;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int HitPoints = 1;
    public string PlayerTag = "Player";

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage()
    {
        HitPoints--;
        if (HitPoints <= 0)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collided with: " + col.collider.tag);
        if(col.collider.CompareTag(PlayerTag)) TakeDamage();
    }
}
