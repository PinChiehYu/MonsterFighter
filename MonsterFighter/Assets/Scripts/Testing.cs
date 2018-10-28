using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : PlayerController
{
    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        GetComponent<Animator>().SetBool("IsGrounded", isGrounded);
        GetComponent<Animator>().SetFloat("SpeedX", velocity.x);
        GetComponent<Animator>().SetFloat("SpeedY", velocity.y);
    }
}
