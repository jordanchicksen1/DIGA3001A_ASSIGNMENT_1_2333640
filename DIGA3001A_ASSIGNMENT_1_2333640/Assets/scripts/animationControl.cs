using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationControl : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator.SetBool("idle", true);
    }

    public void DoJumpAnimation()
    {
        animator.SetBool("jump",true);
        animator.SetBool("idle", false);
        animator.SetBool("linkFlame", false);
        animator.SetBool("walk", false);
    }

    public void DoWalkAnimation() 
    {
        animator.SetBool("walk", true );
        animator.SetBool("idle", false);
        animator.SetBool("linkFlame", false);
        animator.SetBool("jump", false);
    }

    public void DoIdleAnimation()
    {
        animator.SetBool("walk", false);
        animator.SetBool("idle", true);
        animator.SetBool("linkFlame", false);
        animator.SetBool("jump", false);
    }

    public void DoLinkFlameAnimation()
    {
        animator.SetBool("walk", false);
        animator.SetBool("idle", false);
        animator.SetBool("linkFlame", true);
        animator.SetBool("jump", false);
    }

}
