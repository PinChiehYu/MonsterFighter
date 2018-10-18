using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : PhysicsObject {

    private int playerId;
    private Dictionary<string, KeyCode> controlSet;
    private Queue<string> inputBuffer = new Queue<string>();

    private Animator animator;
    
	public override void Awake () {
        base.Awake();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
        /*
        if (Input.GetKeyDown(controlSet["Up"]))
        {
            animator.SetTrigger("Up");
        }
        else if (Input.GetKeyDown(controlSet["Down"]))
        {
            animator.SetTrigger("Down");
        }
        else if (Input.GetKeyDown(controlSet["Right"]))
        {
            animator.SetTrigger("Right");
        }
        else if (Input.GetKeyDown(controlSet["Left"]))
        {
            animator.SetTrigger("Left");
        }
        else if (Input.GetKeyDown(controlSet["Atk_L"]))
        {
            animator.SetTrigger("Atk_L");
        }
        else if (Input.GetKeyDown(controlSet["Atk_H"]))
        {
            animator.SetTrigger("Atk_H");
        }
        */
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            CheckRotate(true);
            velocity.x = 5f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            CheckRotate(false);
            velocity.x = 5f;
        }
        else
        {
            velocity.x = 0f;
        }

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", velocity.x);
    }

    private void CheckRotate(bool Facing)
    {
        if(Facing ^ isFacingRight)
        {
            isFacingRight = Facing;
            transform.Rotate(Vector3.up * 180);
        }
    }

    private void ResetAllAnimatorTrigger()
    {
        //animator.
    }

    public void SetPlayerId(int id)
    {
        playerId = id;
        if (playerId == 1) transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void SetControlSet(Dictionary<string, KeyCode> controlset)
    {
        controlSet = controlset;
    }
}
