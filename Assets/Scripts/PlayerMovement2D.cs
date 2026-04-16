using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Walk Stats")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float acc = 50f;
    [SerializeField] float deacc = 50f;
    [SerializeField] float velPow = 0.9f;

    [Header("Jump Stats")]
    [SerializeField] float jumpForce = 12f;
    bool isJumpPressed;

    [Header("Rope")]
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float ropeHorizontalSnap = 0.2f;
    private Transform currentRope;
    [SerializeField] GameObject floor_parent;

    [Header("Swim Stats")] 
    [SerializeField] private float swimSpeed = 10f;
    
    [Header("Gravity and Fall")]
    [SerializeField] float defaultGravity = 1f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [Header("Ground Detection")]
    [SerializeField] bool debugMode;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    
    
    [Header("Detection Conditions")]
    bool isGrounded;
    bool isTouchingRope;
    bool isClimbing;
    bool isSwimming;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [HideInInspector] Vector2 moveInput;
    
    [Header("Input Condition")]
    [HideInInspector] public bool isAnyInputFired;


    #region Get comps.
    void Awake() => rb = GetComponent<Rigidbody2D>();
    void OnValidate() => rb = GetComponent<Rigidbody2D>();
    #endregion

    #region Unity funcs
    void Update()
    {
        CheckIsPressing();
        CheckGround();
        InputStateCheck();
    }
    #endregion
    
   
    #region  Input
    //Checking for music boxg game
    private void CheckIsPressing()
    {
        if (Mathf.Abs(moveInput.x) > 0 || Mathf.Abs(moveInput.y) > 0) {isAnyInputFired = true;}
        else {isAnyInputFired = false;}
        if (isTouchingRope) 
        {
            if (isClimbing)
            {
                isClimbing = false;
                if (floor_parent != null)
                {
                    Collider2D[] floors = floor_parent.GetComponentsInChildren<Collider2D>();
                    foreach (Collider2D floor in floors)
                    {
                        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), floor, false);
                    }
                }
            }
            else
            {
                isClimbing = true;
                if (floor_parent != null)
                {
                    Collider2D[] floors = floor_parent.GetComponentsInChildren<Collider2D>();
                    foreach (Collider2D floor in floors)
                    {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), floor, true);
                    }
                }
            }
        }

    }

    private void InputStateCheck()
    {
        if (isClimbing)
        {
            Climb();
        }
        else if (isSwimming)
        {
            Swim();
        }
        else
        {
            Walk();
            ApplyBetterGravity();
        }
    }

    #region Input Messages
    //Delegates(messages) from Input
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    public void OnJump(InputValue value) => Jump(value);

    public void OnInteract(InputValue value)
    {
        Debug.Log("Interaction Fired");
        isAnyInputFired = value.isPressed;
        if (isTouchingRope) 
        {
            if (isClimbing) isClimbing = false;
            else isClimbing = true;
            Debug.Log("Touching Rope Interacted!");
        }
    }

    #endregion
    #endregion

    #region Basic Movement
    void Walk()
    {
        float targetSpeed = moveInput.x * maxSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acc : deacc;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif) * Time.deltaTime;

        rb.AddForce(movement * Vector2.right);
    }

    void Jump(InputValue jumpValue)
    {
        isJumpPressed = jumpValue.isPressed;
        isAnyInputFired = jumpValue.isPressed; 
        if (isJumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX,0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Extra Movements

    void Climb()
    {
        rb.gravityScale = 0f;
        if (currentRope != null)
        {
            float targetX = currentRope.position.x;
            float smoothedX = Mathf.Lerp(rb.position.x, targetX, ropeHorizontalSnap * Time.deltaTime);

            rb.linearVelocity = new Vector2(0, moveInput.y * climbSpeed);

            rb.position = new Vector2(smoothedX, rb.position.y);
        }
        
    }

    void Swim()
    {
        rb.gravityScale = 0f;
        float targetSpeedX = moveInput.x * swimSpeed;
        float speedDifX = targetSpeedX - rb.linearVelocityX;
        float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? acc : deacc;
        float movementX = Mathf.Pow(Mathf.Abs(speedDifX) * accelRateX, velPow) * Mathf.Sign(speedDifX) * Time.deltaTime;

        rb.AddForce(movementX * Vector2.right);
        
        float targetSpeedY = moveInput.y * swimSpeed;
        float speedDifY = targetSpeedY - rb.linearVelocityY;
        float accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? acc : deacc;
        float movementY = Mathf.Pow(Mathf.Abs(speedDifY) * accelRateY, velPow) * Mathf.Sign(speedDifY) * Time.deltaTime;

        rb.AddForce(movementY * Vector2.up);
    }
    #endregion

    #region Ground Check & Grav.
    //Do not forget to assign the ground objects' layers to ground Layer.
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position,groundCheckRadius,groundLayer);
    }

    // Curve Fall
    void ApplyBetterGravity()
    {
        if (rb.linearVelocityY < 0) rb.gravityScale = fallMultiplier;
        else if (rb.linearVelocityY > 0 && !isJumpPressed) rb.gravityScale = lowJumpMultiplier;
        else rb.gravityScale = defaultGravity;
    }
    #endregion

    #region Debugging
    void OnDrawGizmos()
    {
        //debug for ground checking
        if (debugMode && groundCheckPos != null)
        {
           Gizmos.color = isGrounded ? Color.green : Color.red;
           Gizmos.DrawWireSphere(groundCheckPos.position,groundCheckRadius); 
        }
    }
    #endregion

    #region Triggers and Collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Rope")) 
        {
            Debug.Log("Touching Rope");
            isTouchingRope = true; 
            currentRope = collision.transform;

        }

        if (collision.CompareTag("Water"))
        {
            Debug.Log("Touching Water");
            isSwimming = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Rope"))
        {
            isTouchingRope = false;
            isClimbing = false;
            currentRope = null;
            rb.gravityScale = defaultGravity;
            if (floor_parent != null)
            {
                Collider2D[] floors = floor_parent.GetComponentsInChildren<Collider2D>();
                foreach (Collider2D floor in floors)
                {
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), floor, false);
                }
            }
        }

        if (collision.CompareTag("Water"))
        {
            isSwimming = false;
            rb.gravityScale = defaultGravity;
        }
    }
    #endregion
}

