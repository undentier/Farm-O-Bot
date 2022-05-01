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
        switch (enable)
        {
            case true:
                animator.SetBool("isWalking", true);
                break;
            case false:
                animator.SetBool("isWalking", false);
                break;
        }
    }
}
