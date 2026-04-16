using System;
using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
     
    [HideInInspector]  bool isPushing;

    private void Awake() => animator = GetComponent<Animator>();

    private void OnValidate()
    {
        if (animator == null) {animator = GetComponent<Animator>();}
    }

    public void TriggerJumpAnimation()
    {
        animator.SetTrigger("Jump");
    }

    public void OnPushAnimation()
    {
        isPushing = true;
        animator.SetBool("Push",isPushing);
    }

    public void OffPushAnimation()
    {
        isPushing = false;
        animator.SetBool("Push", isPushing);
    }

}
