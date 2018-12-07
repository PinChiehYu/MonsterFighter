using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationState : AnimationState
{
    [SerializeField, Header("Combat Settings")]
    private List<CombatSetting> combatList;
    [SerializeField]
    private float switchFrame;
    private int combatId;

    private CombatHandler handler;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        handler = animator.GetComponent<CombatHandler>();
        combatId = -1;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        UpdateCombatParamByFrame();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        handler.CancelAttack();
    }

    private void UpdateCombatParamByFrame()
    {
        if (combatId + 1 < combatList.Count && combatList[combatId + 1].triggerFrame <= currentFrame)
        {
            combatId++;
            combatList[combatId].Execute(handler);
        }
        if (switchFrame >= 0f && switchFrame <= currentFrame)
        {
            controller.TriggerNextCombatState();
        }
    }
}
