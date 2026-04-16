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
        playerRef = GetComponent<PlayerMovement2D>();
    }

    private void Update()
    {
        UpdateWalkAnimation();
        animator.SetBool("Grounded", playerRef.isGrounded);
    }

    private void UpdateWalkAnimation()
    {
        animator.SetFloat("xVelocity", rb.linearVelocityX);
        animator.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void UpdateJumpAnimation()
    {
        
    }

    private void TriggerJumpAnimation()
    {
        animator.SetTrigger("Jump");
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
