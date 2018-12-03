using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : StateMachineBehaviour
{
    [SerializeField, Header("State Settings")]
    private StateType stateType;
    [SerializeField]
    private bool enableBaseInput;
    [SerializeField]
    private bool enableCombatInput;

    [SerializeField, Header("Action Settings")]
    private List<ActionSetting> actionList;
    [SerializeField]
    private bool resetVelocityWhenEnter;
    private int actionId;

    [SerializeField, Header("Combat Settings")]
    private List<CombatSetting> combatList;
    [SerializeField]
    private float switchFrame;
    private int combatId;

    private PlayerController controller;
    private CombatHandler combatHandler;
    private PhysicsObject physics;
    private float currentFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        controller = animator.GetComponent<PlayerController>();
        combatHandler = animator.GetComponent<CombatHandler>();
        physics = animator.GetComponent<PhysicsObject>();
        actionId = combatId = -1;

        controller.CurrentState = stateType;
        controller.EnableBaseInput = enableBaseInput;
        controller.EnableCombatInput = enableCombatInput;

        if (resetVelocityWhenEnter)
        {
            physics.SetPhysicsParam(Vector2.zero, Vector2.zero, true);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentFrame = (stateInfo.normalizedTime % 1f) * stateInfo.length * 15;
        if (actionList.Count > 0)
        {
            UpdatePhysicsParamByFrame();
        }
        UpdateCombatParamByFrame();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        physics.SetPhysicsParam(null, Vector2.zero, true);
        combatHandler.CancelAttack();
    }

    private void UpdatePhysicsParamByFrame()
    {
        if (actionId+1 < actionList.Count && actionList[actionId+1].triggerFrame <= currentFrame)
        {
            actionId++;
            physics.SetPhysicsParam(actionList[actionId].initVelocity, actionList[actionId].acceleration, actionList[actionId].useDefaultGravity);
        }
    }

    private void UpdateCombatParamByFrame()
    {
        if (combatId + 1 < combatList.Count && combatList[combatId + 1].triggerFrame <= currentFrame)
        {
            combatId++;
            combatList[combatId].Execute(combatHandler);
        }
        if (switchFrame >= 0f && switchFrame <= currentFrame)
        {
            controller.TriggerNextCombatState();
        }
    }
}
