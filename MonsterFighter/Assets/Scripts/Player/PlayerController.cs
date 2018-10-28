using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(PlayerInfo))]
public class PlayerController : PhysicsObject {

    [SerializeField]
    private ActionList actionList;

    private PlayerInfo playerInfo;
    private Animator animator;

    private Vector2 initPosition;
    private Dictionary<string, KeyCode> controlSet;
    private Queue<string> inputBuffer = new Queue<string>();


    private AtkTrigger atkTrigger;
    
	public override void Awake () {
        base.Awake();
        /////////for Sene Test///////
        string[] controlType = new string[] { "Up", "Down", "Right", "Left", "AtkL", "AtkH" };
        string[] defaultControlKeyCode = new string[] { "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Comma", "Period" };
        controlSet = new Dictionary<string, KeyCode>();
        for (int j = 0; j < controlType.Length; j++)
        {
            string defaultKeyCode = PlayerPrefs.GetString("0_" + controlType[j], defaultControlKeyCode[j]);
            controlSet.Add(controlType[j], (KeyCode)Enum.Parse(typeof(KeyCode), defaultKeyCode));
        }
        /////////for Sene Test///////
        playerInfo = GetComponent<PlayerInfo>();
        animator = GetComponent<Animator>();
	}

    void Update () {
        HandleMovementOperation();
        HandleCombatOperation();
        UpdateAnimator();
    }

    private void HandleMovementOperation()
    {
        if (playerInfo.actionState == ActionState.Action)
        {
            return;
        }

        if (Input.GetKey(controlSet["Right"]))
        {
            CheckFaceRight(true);
            velocity.x = 5f;
        }
        else if (Input.GetKey(controlSet["Left"]))
        {
            CheckFaceRight(false);
            velocity.x = 5f;
        }

        if (Input.GetKeyDown(controlSet["Up"]) && isGrounded)
        {
            Jump();
        }
        else if (Input.GetKey(controlSet["Down"]) && !isGrounded)
        {
            Fall();
        }
    }

    private void HandleCombatOperation()
    {
        if (playerInfo.actionState == ActionState.Normal)
        {
            animator.ResetTrigger("AtkL");
            animator.ResetTrigger("AtkH");
            atkTrigger = AtkTrigger.None;
        }

        if (Input.GetKeyDown(controlSet["AtkH"]))
        {
            atkTrigger = AtkTrigger.AtkH;
        }
        else if (Input.GetKeyDown(controlSet["AtkL"]))
        {
            atkTrigger = AtkTrigger.AtkL;
        }

        if (playerInfo.actionState == ActionState.Normal || playerInfo.combatState == CombatState.Transition)
        {
            string trigger = Enum.GetName(typeof(AtkTrigger), atkTrigger);
            if (trigger != "None")
            {
                animator.SetTrigger(trigger);
                atkTrigger = AtkTrigger.None;
            }
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("SpeedX", velocity.x);
        animator.SetFloat("SpeedY", velocity.y);
    }

    public void SetupController(Dictionary<string, KeyCode> controlset, Vector2 initposition)
    {
        controlSet = controlset;
        initPosition = initposition;
        GetComponentInChildren<AttackboxDetector>().id = playerInfo.id;
    }

    public void InitController()
    {
        transform.position = initPosition;
        velocity = Vector2.zero;
        isGrounded = false;
        CheckFaceRight(playerInfo.id == 0);
        animator.Rebind();
    }

    private void CheckFaceRight(bool newFacing)
    {
        if (newFacing ^ isFaceRight)
        {
            isFaceRight = newFacing;
            transform.Rotate(Vector3.up * 180);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 50), Enum.GetName(typeof(CombatState), playerInfo.combatState));
    }

    private void Jump()
    {
        isGrounded = false;
        velocity.y = jumpVelocity;
    }

    private void Fall()
    {
        if (velocity.y > 0f)
        {
            velocity.y = 0f;
        }
    }

    public void Damaged(float enemyXPosition)
    {
        CheckFaceRight(enemyXPosition > transform.position.x);
        velocity.x = -10f;
        animator.SetTrigger("Damaged");
    }

    public void Fallout()
    {
        transform.position = new Vector2(-4f, -1f);
        velocity.x = 0f;
        animator.SetTrigger("Damaged");
    }

    private float GetCurrentFrame()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f);
    }
}
