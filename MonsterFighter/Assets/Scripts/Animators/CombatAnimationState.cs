using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationState : BaseAnimationState
{
    [SerializeField]
    private List<CombatInfo> combatList;
    private int combatId;

    private CombatHandler combatHandler;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        combatHandler = animator.GetComponent<CombatHandler>();
        combatId = -1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        UpdateCombatParamByFrame();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    private void UpdateCombatParamByFrame()
    {
        if (combatId + 1 < combatList.Count && combatList[combatId + 1].triggerFrame < currentFrame)
        {
            combatId++;
            combatHandler.PrepareAttack(combatList[combatId]);
        }
    }
}
