using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(PlayerInfo), typeof(PhysicsObject))]
public class PlayerController : MonoBehaviour {

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

    private IEnumerator damageCo;
    public void Damaged(Vector2 applyVelocity, float stiffTime, bool isKnockDown, float enemyXPosition)
    {
        physics.IsFaceRight = enemyXPosition > transform.position.x;
        EnableBaseInput = false;
        if (damageCo != null) StopCoroutine(damageCo);
        physics.SetPhysicsParam(applyVelocity, Vector2.zero, true);
        damageCo = Stiff(stiffTime, isKnockDown);
        StartCoroutine(damageCo);
    }

    public void Fallout()
    {
        transform.position = new Vector2(-4f, -1f);
        EnableBaseInput = false;
        EnableCombatInput = false;
        physics.Forward(0f);
        if (damageCo != null) StopCoroutine(damageCo);
        StartCoroutine(Stiff(0f, true));
    }

    IEnumerator Stiff(float duration, bool knockDown)
    {
        animator.ResetTrigger("WakeUp");
        animator.SetTrigger("Damaged");
        yield return new WaitForSeconds(duration);
        if (knockDown)
        {
            physics.SetPhysicsParam(Vector2.zero, Vector2.zero, true);
            transform.Find("Damage").gameObject.SetActive(false);
            animator.SetTrigger("KnockDown");
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("WakeUp");
            yield return new WaitForSeconds(1f);
            transform.Find("Damage").gameObject.SetActive(true);
        }
        else
        {
            animator.SetTrigger("WakeUp");
        }
    }
}
