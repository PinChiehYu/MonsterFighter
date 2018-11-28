using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimationState : StateMachineBehaviour
{
    [SerializeField]
    private List<ActionInfo> actionList;
    [SerializeField]
    private bool resetVelocity;

    private PhysicsObject physics;
    private int actionId;

    protected float currentFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        physics = animator.GetComponent<PhysicsObject>();
        actionId = -1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        currentFrame = (stateInfo.normalizedTime % 1f) * stateInfo.length * 15;
        UpdatePhysicsParamByFrame();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (resetVelocity)
        {
            physics.SetPhysicsParam(Vector2.zero, Vector2.zero, true);
        }
        else
        {
            physics.SetPhysicsParam(null, Vector2.zero, true);
        }
    }

    private void UpdatePhysicsParamByFrame()
    {
        if (actionId+1 < actionList.Count && actionList[actionId+1].triggerFrame < currentFrame)
        {
            actionId++;
            physics.SetPhysicsParam(actionList[actionId].initVelocity, actionList[actionId].acceleration, actionList[actionId].useDefaultGravity);
        }
    }
}
