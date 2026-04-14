using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Walk Stats")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float acc = 50f;
    [SerializeField] float deAcc = 50f;
    [SerializeField] float velPow = 0.9f;

    [Header("Jump Stats")]
    [SerializeField] float jumpForce = 12f;
    bool isJumpPressed;

    [Header("Gravity and Fall")]
    [SerializeField] float defaultGravity = 1f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [Header("Ground Detection")]
    [SerializeField] bool debugMode;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 moveInput;



    #region validation for critic comps.    
    void Awake() => rb = GetComponent<Rigidbody2D>();
    void OnValidate() => rb = GetComponent<Rigidbody2D>();
    #endregion
    
    //Delegates(messages) from Input
    #region  Input
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    public void OnJump(InputValue value) => Jump(value);

    #endregion
    
    void Update()
    {
       Walk();
       ApplyBetterGravity();
       CheckGround();
    }
  
    #region Basic Mov.
    void Walk()
    {
        float targetSpeed = moveInput.x * maxSpeed;
        
        float speedDif = targetSpeed - rb.linearVelocityX;
       
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acc : deAcc;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif) * Time.deltaTime;

        rb.AddForce(movement * Vector2.right);
    }

    void Jump(InputValue jumpValue)
    {
        isJumpPressed = jumpValue.isPressed;
        if (isJumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX,0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    #endregion

    #region Ground Check & Grav.
    //Do not forget to assign the ground objects' layers to ground Layer.
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
    }

    

    void ApplyBetterGravity()
    {
        if (rb.linearVelocityY < 0) rb.gravityScale = fallMultiplier;
        else if (rb.linearVelocityY > 0 && !isJumpPressed) rb.gravityScale = lowJumpMultiplier;
        else rb.gravityScale = defaultGravity;
    }
    #endregion

    void OnDrawGizmos()
    {
        //debug for ground checking
        if (debugMode && groundCheck != null)
        {
           Gizmos.color = isGrounded ? Color.green : Color.red;
           Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius); 
        }
    }
}

