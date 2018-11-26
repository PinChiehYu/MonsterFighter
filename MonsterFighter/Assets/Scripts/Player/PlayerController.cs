using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInfo), typeof(PhysicsObject))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private ActionList actionList;

    private PlayerInfo playerInfo;
    private PhysicsObject physics;
    private Animator animator;

    private Vector2 initPosition;
    private Dictionary<string, KeyCode> controlSet;
    private Queue<string> inputBuffer = new Queue<string>();


    private AtkTrigger atkTrigger;
    
	public void Awake () {
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
        physics = GetComponent<PhysicsObject>();
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
            physics.IsFaceRight = true;
            physics.Forward(5f);
        }
        else if (Input.GetKey(controlSet["Left"]))
        {
            physics.IsFaceRight = false;
            physics.Forward(5f);
        }
        else if (physics.IsGrounded)
        {
            physics.Forward(0f);
        }

        if (Input.GetKeyDown(controlSet["Up"]) && physics.IsGrounded)
        {
            physics.Jump();
        }
        else if (Input.GetKey(controlSet["Down"]) && physics.IsGrounded)
        {
            
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
        animator.SetBool("IsGrounded", physics.IsGrounded);
        animator.SetFloat("SpeedX", physics.Velocity.x);
        animator.SetFloat("SpeedY", physics.Velocity.y);
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
        physics.IsFaceRight = playerInfo.id == 0;
        animator.Rebind();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 50), Enum.GetName(typeof(CombatState), playerInfo.combatState));
    }

    public void Damaged(float enemyXPosition)
    {
        physics.IsFaceRight = enemyXPosition > transform.position.x;
        physics.Forward(-10f);
        animator.SetTrigger("Damaged");
    }

    public void Fallout()
    {
        transform.position = new Vector2(-4f, -1f);
        physics.Forward(0f);
        animator.SetTrigger("Damaged");
    }

    private float GetCurrentFrame()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f);
    }
}
