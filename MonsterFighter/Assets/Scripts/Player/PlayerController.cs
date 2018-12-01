using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInfo), typeof(PhysicsObject))]
public class PlayerController : MonoBehaviour {

    private PlayerInfo playerInfo;
    private PhysicsObject physics;
    private Animator animator;

    private Vector2 initPosition;
    private Dictionary<string, KeyCode> controlSet;
    private Queue<string> inputBuffer = new Queue<string>();

    public StateType CurrentState { get; set; }
    public bool EnableBaseInput { get; set; }
    public bool EnableCombatInput { get; set; }

    private CombatTrigger combatTrigger;
    
	public void Awake () {
        /////////for Sene Test///////
        string[] controlType = new string[] { "Up", "Down", "Right", "Left", "AtkL", "AtkH", "SklS", "SklB" };
        string[] defaultControlKeyCode = new string[] { "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Comma", "Period", "K", "L" };
        controlSet = new Dictionary<string, KeyCode>();
        for (int j = 0; j < controlType.Length; j++)
        {
            controlSet.Add(controlType[j], (KeyCode)Enum.Parse(typeof(KeyCode), defaultControlKeyCode[j]));
        }
        /////////for Sene Test///////
        playerInfo = GetComponent<PlayerInfo>();
        physics = GetComponent<PhysicsObject>();
        animator = GetComponent<Animator>();
	}

    void Update () {
        HandleBaseOperation();
        HandleCombatOperation();
        UpdateAnimator();
    }

    private void HandleBaseOperation()
    {
        if (!EnableBaseInput) return;

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
    }

    private void HandleCombatOperation()
    {
        if (!EnableCombatInput) return;

        if (CurrentState == StateType.Base)
        {
            ResetTriggers();
            combatTrigger = CombatTrigger.None;
        }

        if (Input.GetKeyDown(controlSet["AtkH"]))
        {
            combatTrigger = CombatTrigger.AtkH;
        }
        else if (Input.GetKeyDown(controlSet["AtkL"]))
        {
            combatTrigger = CombatTrigger.AtkL;
        }
        else if (Input.GetKey(controlSet["SklS"]))
        {
            combatTrigger = CombatTrigger.SklS;
        }
        else if (Input.GetKey(controlSet["SklB"]))
        {
            combatTrigger = CombatTrigger.SklB;
        }

        if (CurrentState == StateType.Base)
        {
            TriggerNextCombatState();
        }
    }

    private void ResetTriggers()
    {
        animator.ResetTrigger("AtkL");
        animator.ResetTrigger("AtkH");
        animator.ResetTrigger("SklS");
        animator.ResetTrigger("SklB");
    }

    public void TriggerNextCombatState()
    {
        string trigger = Enum.GetName(typeof(CombatTrigger), combatTrigger);
        if (trigger != "None")
        {
            animator.SetTrigger(trigger);
            combatTrigger = CombatTrigger.None;
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
    }

    public void ResetController()
    {
        transform.position = initPosition;
        physics.IsFaceRight = gameObject.name == "0";
        physics.IsGrounded = false;
        EnableBaseInput = EnableCombatInput = true;
        GetComponentInChildren<AttackboxDetector>().Id = gameObject.name;
        animator.Rebind();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 15f * int.Parse(gameObject.name), 200f, 50f), "Player " + gameObject.name + " Current:" + EnableBaseInput.ToString());
    }

    public void Damaged(Vector2 applyVelocity, float enemyXPosition)
    {
        physics.IsFaceRight = enemyXPosition > transform.position.x;
        EnableBaseInput = false;
        physics.SetPhysicsParam(applyVelocity, Vector2.zero, true);
        animator.SetTrigger("Damaged");
    }

    public void Fallout()
    {
        transform.position = new Vector2(-4f, -1f);
        physics.Forward(0f);
        animator.SetTrigger("Damaged");
    }
}
