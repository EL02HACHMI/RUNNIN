using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private Vector2 moveInput;
    private bool _isFacingRight = true; // Corrected the property name

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving)
            {
                return IsRunning ? runSpeed : walkSpeed;
            }
            return 0; // Idle Speed is 0
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            animator.SetBool("isRunning", value);
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1); // Corrected the Vector constructor
            }
        }
    }

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput); // Moved SetFacingDirection call to here with parameter
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !_isFacingRight) // Corrected the condition check
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && _isFacingRight) // Corrected the condition check
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        IsRunning = context.ReadValueAsButton();
    }
}
