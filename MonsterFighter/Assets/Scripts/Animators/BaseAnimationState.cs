using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimationState : StateMachineBehaviour
{
    public List<ActionInfo> actionList;
    private PhysicsObject physics;
    private float currentFrame;
    private int actionId;

    public bool resetVelocity;

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
            physics.SetPhysicsParam(Vector2.zero, null);
        }
        else
        {
            physics.SetPhysicsParam(null, null);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    private void UpdatePhysicsParamByFrame()
    {
        if (actionId+1 < actionList.Count && actionList[actionId+1].triggerFrame < currentFrame)
        {
            actionId++;
            if (actionList[actionId].useDefaultGravity)
            {
                physics.SetPhysicsParam(actionList[actionId].initVelocity, null);
            }
            else
            {
                physics.SetPhysicsParam(actionList[actionId].initVelocity, actionList[actionId].initAcceleration);
            }
        }
    }
}
