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
        StartCoroutine(ClearJump());
        
    }

    public void DoWalkAnimation() 
    {
        animator.SetBool("walk", true );
        StartCoroutine(ClearWalk());
    }

    public void DoIdleAnimation()
    {
        
        animator.SetBool("idle", true);
        StartCoroutine(ClearIdle());
    
    }

    public void DoLinkFlameAnimation()
    {
        
        animator.SetBool("linkFlame", true);
        StartCoroutine(ClearLink());
        
    }

    public IEnumerator ClearJump() 
    {
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("jump", false );
    }

    public IEnumerator ClearLink()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("linkFlame", false);
    }
    public IEnumerator ClearWalk()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("walk", false);
    }

    public IEnumerator ClearIdle()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("idle", false);
    }


}
