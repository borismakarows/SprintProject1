using System;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class AnimationController : MonoBehaviour
{
    [Header("Conditions")]
    [HideInInspector]  bool isPushing;
    [HideInInspector] public bool wasGroundedLastFrame;

    [Header("Components")]
    [SerializeField] Animator animator;
    private Rigidbody2D rb;
    [SerializeField] PlayerMovement2D playerRef;

    private void OnValidate()
    {
        if (animator == null) {animator = GetComponent<Animator>();}
    }

    private void Awake() 
    { 
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        playerRef = GetComponentInParent<PlayerMovement2D>();
    }

    private void Update()
    {
        UpdateWalkAnimation();
        UpdateJumpAnimation();
        UpdateSwim();
    }

    private void RoomCheck()
    {
        animator.SetBool("InBunnyRoom", playerRef.currentRoom == Room.BunnyPlushie);
        animator.SetBool("InMusicRoom", playerRef.currentRoom == Room.MusicBox);
        animator.SetBool("Swimming", playerRef.currentRoom == Room.Piranhas);
    }

    private void UpdateWalkAnimation()
    {
        animator.SetFloat("xVelocityAbs", MathF.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
    }
    
    private void UpdateJumpAnimation()
    {
        RoomCheck();
        animator.SetBool("Ground", playerRef.isGrounded);
        animator.SetBool("Jumping", playerRef.isJumping);
        wasGroundedLastFrame = playerRef.isGrounded;
    }

    private void UpdateSwim()
    {
        int moveInputX = Mathf.RoundToInt(playerRef.moveInput.x);
        animator.SetInteger("XMoveInput", moveInputX);
    }

    public void PushAnimationOn()
    {
        isPushing = true;
        animator.SetBool("Push",isPushing);
    }

    public void PushAnimationOff()
    {
        isPushing = false;
        animator.SetBool("Push", isPushing);
    }

    public void ChangeRunTimeAnimator(RuntimeAnimatorController newAnimatorCtr)
    {
        if (newAnimatorCtr != null)
        {
            animator.runtimeAnimatorController = newAnimatorCtr; 
            Debug.Log("Era Animation Changed to: " + newAnimatorCtr.name);
        }
       
    }
}
