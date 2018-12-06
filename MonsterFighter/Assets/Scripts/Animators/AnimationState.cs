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

    protected PlayerController controller;
    private PhysicsObject physics;

    protected float currentFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<PlayerController>();
        physics = animator.GetComponent<PhysicsObject>();
        actionId = -1;

        controller.CurrentState = stateType;
        controller.SetInputActivate(enableBaseInput, enableCombatInput);

        if (resetVelocityWhenEnter)
        {
            physics.SetPhysicsParam(Vector2.zero, Vector2.zero, true);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentFrame = (stateInfo.normalizedTime % 1f) * stateInfo.length * 15;
        UpdatePhysicsParamByFrame();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        physics.SetPhysicsParam(null, Vector2.zero, true);
    }

    private void UpdatePhysicsParamByFrame()
    {
        if (actionId+1 < actionList.Count && actionList[actionId+1].triggerFrame <= currentFrame)
        {
            actionId++;
            physics.SetPhysicsParam(actionList[actionId].initVelocity, actionList[actionId].acceleration, actionList[actionId].useDefaultGravity);
        }
    }
}
