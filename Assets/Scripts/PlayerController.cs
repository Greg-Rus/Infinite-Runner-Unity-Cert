using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    

    private float _horizontalMovementInput = 0f;
    private bool _jumpInput = false;
    private bool _grounded = false;
    private bool _isAirborne = false;

    [Header("Grounding Settings")]
    public Transform GroundCheckAnchor;
    public float GroundCheckRadius = 0.1f;
    public LayerMask GroundLayers;

    [Header("Movement Settings")]
    public float PlayerSpeed = 2f;
    public float JumpStrength = 3f;

    [Header("Colliders")]
    public BoxCollider2D FeetCollider;
    public BoxCollider2D RightCollider;
    public BoxCollider2D LeftCollider;
    // Use this for initialization
    void Start ()
	{
	    _rigidbody2D = GetComponent<Rigidbody2D>();
	    _animator = GetComponent<Animator>();

	}

    void FixedUpdate()
    {
        
    }

	void Update ()
	{
	    ReadInput();
        ProcessInput();
	}

    private void ReadInput()
    {
        _horizontalMovementInput = Input.GetAxis("Horizontal");
        _jumpInput = Input.GetKeyDown(KeyCode.Space);
        _grounded = IsGrounded();
    }

    private void ProcessInput()
    {
        _animator.SetBool("Grounded", _grounded);
        if (_jumpInput && _grounded) _rigidbody2D.AddForce(Vector2.up * JumpStrength);
        _rigidbody2D.velocity = new Vector2(_horizontalMovementInput * PlayerSpeed, _rigidbody2D.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(_horizontalMovementInput));
    }

    private bool IsGrounded()
    {
        return FeetCollider.IsTouchingLayers(GroundLayers);
    }
}
