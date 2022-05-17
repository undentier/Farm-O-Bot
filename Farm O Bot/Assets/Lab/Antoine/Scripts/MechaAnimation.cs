using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void WalkAnimation(bool enable)
    {
        if(enable) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);
    }

    public void RunAnimation(bool enable)
    {
        if (enable) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
    }
}
