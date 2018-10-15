using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float FollowThreshold = 0.1f;
    public float CameraCatchupStep = 0.5f;

    private Transform _cameraTransform;
	// Use this for initialization
	void Start ()
	{
	    _cameraTransform = GetComponent<Transform>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    _cameraTransform.position = _cameraTransform.position + GetYCameraOffset();

    }

    private Vector3 GetYCameraOffset()
    {
        Vector2 cameraDirection = Target.position - _cameraTransform.position;
        float travelDistance = cameraDirection.magnitude * CameraCatchupStep;
        if (travelDistance <= FollowThreshold)
        {
            return Vector3.zero;
        }
        return cameraDirection.normalized * travelDistance;

    }
}
