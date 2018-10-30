using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapMover : MonoBehaviour
{
    public Transform Player;

    public float MinimapLead;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var newMiniMapCameraPosition = Player.position;
	    newMiniMapCameraPosition.x += MinimapLead;
	    newMiniMapCameraPosition.z = -10;
	    transform.position = newMiniMapCameraPosition;
	}
}
