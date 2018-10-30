using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TagController : MonoBehaviour
{
    private Transform _tagTransform;
    private Transform _parent;
    private Camera _minimapCamera;
	// Use this for initialization
	void Start ()
	{
	    _tagTransform = transform;
	    _minimapCamera = FindObjectsOfType<Camera>().SingleOrDefault(camera => camera.CompareTag("MiniMapCamera"));
	    _parent = _tagTransform.parent.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    _tagTransform.transform.rotation = Quaternion.identity;
	    Vector3[] frustumCorners = new Vector3[4];
	    _minimapCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), _minimapCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
	    Vector3[] cornersInWorldSpace =
	        frustumCorners.Select(corner => _minimapCamera.transform.TransformVector(corner) + _minimapCamera.transform.position).ToArray();
	    var rightScreenEdge = cornersInWorldSpace.Max(edge => edge.x);
	    var leftScreenEdge = cornersInWorldSpace.Min(edge => edge.x);
	    if (_parent.position.x > rightScreenEdge)
	    {
	        MoveTagToEdge(rightScreenEdge);

	    }
        else if (_parent.position.x < leftScreenEdge)
	    {
	        MoveTagToEdge(leftScreenEdge);
	    }
	    else
	    {
	        _tagTransform.position = _parent.position;
	    }

	}

    private void MoveTagToEdge(float edgePosition)
    {
        var newPosition = _parent.position;
        newPosition.x = edgePosition;
        _tagTransform.position = newPosition;
    }
}
