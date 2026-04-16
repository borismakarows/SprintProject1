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
        RoomCheck();
        UpdateWalkAnimation();
        UpdateJumpAnimation();
    }

    private void RoomCheck()
    {
        animator.SetBool("InBunnyRoom", playerRef.isInBunnyRoom);
        animator.SetBool("InMusicRoom", playerRef.isInMusicBoxRoom);
    }

    private void UpdateWalkAnimation()
    {
        animator.SetFloat("xVelocityAbs", MathF.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void UpdateJumpAnimation()
    {
        animator.SetBool("Ground", playerRef.isGrounded);
        animator.SetBool("Jumping", playerRef.isJumping);
        wasGroundedLastFrame = playerRef.isGrounded;
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

}
