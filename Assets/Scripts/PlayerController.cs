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
    private GameObject _stompedEnemy;

    [Header("Grounding Settings")]
    public Transform GroundCheckAnchor;
    public float GroundCheckRadius = 0.1f;
    public LayerMask GroundLayers;
    public LayerMask EnemyLayers;

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
        if (_jumpInput && _grounded) Jump();
        _rigidbody2D.velocity = new Vector2(GetPossibleVelocity(_horizontalMovementInput) * PlayerSpeed, _rigidbody2D.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(_horizontalMovementInput));
    }

    private void Jump()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(Vector2.up * JumpStrength);
    }

    private float GetPossibleVelocity(float horizontalInput)
    {
        if (horizontalInput > 0 && RightCollider.IsTouchingLayers(GroundLayers))
        {
            return 0;
        }
        if (horizontalInput < 0 && LeftCollider.IsTouchingLayers(GroundLayers))
        {
            return 0;
        }
        return horizontalInput;
    }

    private bool IsGrounded()
    {
        return FeetCollider.IsTouchingLayers(GroundLayers);
    }

    private bool StompingEnemy()
    {
        return FeetCollider.IsTouchingLayers(EnemyLayers);
    }

    public void TeleportPlayer(Vector2 targetPosition)
    {
        _rigidbody2D.MovePosition(targetPosition);
        _rigidbody2D.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (StompingEnemy())
        {
            Jump();
            col.gameObject.GetComponent<Enemy>().TakeDamage();
        }
    }
}
