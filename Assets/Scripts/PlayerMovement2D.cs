using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Room
{
    TeenHouse,
    BunnyPlushie,
    MusicBox,
    Piranhas
}

public enum Era
{
    Child,
    Teen
}

[RequireComponent(typeof(Rigidbody2D),typeof(PlayerInput))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Era Information")]
    public Era currentEra = Era.Child; 
    public EraStats childStats;
    public EraStats teenStats;

    // Do not forget to set them true when entering a room
    [Header("Room Information")]
    public Room currentRoom = Room.TeenHouse;

    [Header("Walk Stats")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float acc = 50f;
    [SerializeField] float deacc = 50f;
    [SerializeField] float velPow = 0.9f;

    [Header("Jump Stats")]
    [SerializeField] float jumpForce = 12f;
	[SerializeField] float bunnyJumpMult = 5f;
    [HideInInspector] public bool isJumping;

    [Header("Rope")]
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float ropeHorizontalSnap = 0.2f;
    private Transform currentRope;

    [Header("Swim Stats")] 
    [SerializeField] private float swimSpeed = 10f;
    
    [Header("Gravity and Fall")]
    [SerializeField] float defaultGravityScale = 1f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [Header("Ground Detection")]
    [SerializeField] bool debugMode;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    
    [Header("Detection Conditions")]
    [HideInInspector] public bool isGrounded;
    bool isTouchingRope;
    bool isClimbing;
    [HideInInspector] public bool isSwimming;
    bool canPush = true;
    

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AnimationController animCtr;
    private SpriteRenderer spriteRenderer;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] UIManager uIManager;
    
    [Header("Input")]
    [HideInInspector] public bool isMoving;
    [HideInInspector] public Vector2 moveInput;


    #region Get comps.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animCtr = GetComponentInChildren<AnimationController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        DebugComponents();
        if (currentRoom == Room.TeenHouse) {SwitchEra(Era.Teen);}
        else {SwitchEra(Era.Child);}
    }

    void OnValidate() => rb = GetComponent<Rigidbody2D>();
    #endregion

    #region Unity funcs
    void Update()
    {
        CheckAnyMoveInputs();
        CheckGround();
    }

    void FixedUpdate()
    {
        CheckMoveState();
    }

    #endregion


    #region  Input
    //Checking for music boxg game
    private void CheckAnyMoveInputs()
    {
        isMoving = (Mathf.Abs(rb.linearVelocityX) > 0.01f || Mathf.Abs(rb.linearVelocityY) > 0.01f || isJumping) ? true : false;
    }
    
    private void CheckMoveState()
    {
        if (isClimbing)
        {
            Climb();
        }
        else
        {
            Walk();
            ApplyBetterGravity();
            if (!isSwimming) rb.gravityScale = defaultGravityScale; 

        }
        if (isSwimming) Swim();
        
    }

    public void SwitchToUIInput()
    {
        playerInput.SwitchCurrentActionMap("UI");
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None;
    }

    public void SwitchToGameInput()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion
    #region Input Messages
    //Delegates(messages) from Input
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    public void OnJump(InputValue value) => Jump(value);

    public void OnInteract(InputValue value)
    {
        if (value.isPressed && isTouchingRope)
        {
            ToggleClimbing(!isClimbing);
            Debug.Log("Climbing Toggled: " + isClimbing);
        }
    }

    public void OnPause(InputValue value)
    {
        if (value.isPressed && uIManager != null)
        {
            uIManager.TogglePause();
        }
    }

    
    #endregion

    #region Basic Movement
    void Walk()
    {
       SpriteFlip();

        float targetSpeed = moveInput.x * maxSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acc : deacc;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }

    private void SpriteFlip()
    {
        if (moveInput.x > 0) {spriteRenderer.flipX = true;}
        else if (moveInput.x < 0) {spriteRenderer.flipX = false;} 
    }

    void Jump(InputValue jumpValue)
    {
        //Player cannot jump if it is not in a jumpable room   
        bool isInMiniGame = currentRoom == Room.BunnyPlushie || currentRoom == Room.MusicBox;
        if (isInMiniGame && currentEra == Era.Child) 
        {
            isJumping = jumpValue.isPressed;
            if (isJumping && isGrounded)
            {
                JumpFunc();
                isGrounded = false;
            }  
        }
        else 
        {Debug.Log("Can't jump because: 1.Not in a room or \n2.you are teen: Everyone knows teens do not jump but lowkey wants. c'mon buddy, no need to cry.");}
        
    }

    void JumpFunc()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX,0);
		if(currentRoom == Room.BunnyPlushie){
			rb.AddForce(Vector2.up * jumpForce * bunnyJumpMult, ForceMode2D.Impulse);
        }
		else{
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
            //player moves to the center of the rope
            float smoothedX = Mathf.Lerp(rb.position.x, targetX, ropeHorizontalSnap * Time.deltaTime);

            rb.linearVelocity = new Vector2(0, moveInput.y * climbSpeed);

            rb.position = new Vector2(smoothedX, rb.position.y);
        } 
    }

    private void ToggleClimbing(bool currState)
    {
        isClimbing = currState;
        rb.gravityScale = currState ? 0 : defaultGravityScale;
            GameObject[] floors = GameObject.FindGameObjectsWithTag("WalkthroughFloor");
            Collider2D playerCol = GetComponent<Collider2D>();
            foreach (GameObject floor in floors)
            {
                Collider2D floorCol = floor.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(playerCol, floorCol, currState);
            }
        if (!currState) rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
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
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position,groundCheckRadius,groundLayer);
    }

    // Curve Fall
    void ApplyBetterGravity()
    {
        if (rb.linearVelocityY < 0) rb.gravityScale = fallMultiplier;
        else if (rb.linearVelocityY > 0 && !isJumping) rb.gravityScale = lowJumpMultiplier;
        else rb.gravityScale = defaultGravityScale;
    }
    #endregion
    
    #region Era Control
    public void SwitchEra(Era newEra)
    {
        currentEra = newEra;
        
        if (newEra == Era.Child)
        {SwitchToChild();}
        else {SwitchToTeen();}
    }

    private void SwitchToChild()
    {
        currentEra = Era.Child;
        //Movement
        maxSpeed = childStats.maxSpeed;
        acc = childStats.acc;
        deacc = childStats.deacc;
        velPow = childStats.velPow;
        //Verticality
        fallMultiplier = childStats.fallMultiplier;
        lowJumpMultiplier = childStats.lowJumpMultiplier;
        climbSpeed = childStats.climbSpeed;
        defaultGravityScale = childStats.gravityScale;
        //Animation
        animCtr.ChangeRunTimeAnimator(childStats.animator);
        spriteRenderer.sprite = childStats.sprite;
    }
    
        private void SwitchToTeen()
    {
        currentEra = Era.Teen;
        //Movement
        maxSpeed = teenStats.maxSpeed;
        acc = teenStats.acc;
        deacc = teenStats.deacc;
        velPow = teenStats.velPow;
        //Verticality
        fallMultiplier = teenStats.fallMultiplier;
        lowJumpMultiplier = teenStats.lowJumpMultiplier;
        climbSpeed = teenStats.climbSpeed;
        defaultGravityScale = teenStats.gravityScale;
        //Animation
        animCtr.ChangeRunTimeAnimator(teenStats.animator);
        spriteRenderer.sprite = teenStats.sprite;
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

    private void DebugComponents()
    {
        if (animCtr == null) {Debug.Log("Anim controller is missing!");}
        if (spriteRenderer == null) {Debug.Log("spire render comp. is missing!");}
        if (playerInput == null) {Debug.Log("Fuck, no player input");}
        if (uIManager == null) {Debug.Log("no UI Manager in player script");}
    }
    #endregion

    #region Triggers 
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

        if (collision.CompareTag("Box"))
        {
            canPush = false;
            animCtr.PushAnimationOff();
        }
        if (collision.CompareTag("Minigame"))
        {
            SwitchEra(Era.Child);
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Rope"))
        {
            isTouchingRope = false;
            isClimbing = false;
            currentRope = null;
            rb.gravityScale = defaultGravityScale;
            GameObject[] floors = GameObject.FindGameObjectsWithTag("WalkthroughFloor");
            foreach (GameObject floor in floors)
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), floor.GetComponent<Collider2D>(), false);
            }
        }

        if (collision.CompareTag("Water"))
        {
            isSwimming = false;
            rb.gravityScale = defaultGravityScale;
        }
        
        if (collision.CompareTag("Box"))
        {
            canPush = true;
        }
    }

    #endregion
    
    #region Collisions
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (!canPush) return;
            Debug.Log("Touch Box");
            animCtr.PushAnimationOn();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            animCtr.PushAnimationOff();
        }
    }
    #endregion
}

