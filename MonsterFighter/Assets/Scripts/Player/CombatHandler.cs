using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo), typeof(PlayerController))]
public class CombatHandler : MonoBehaviour, ICombatSender {

    private PlayerInfo playerInfo;
    private PlayerController playerController;

    private int comboCounter;

    private bool readyToAttack;
    private CombatInfo combatInfo;

    public event Action OnHitTarget;

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        playerController = GetComponent<PlayerController>();
        comboCounter = 0;
        readyToAttack = false;
    }

    public void SendAttack(CombatHandler enemyHandler)
    {
        if (readyToAttack)
        {
            comboCounter++;
            OnHitTarget?.Invoke();
            enemyHandler.ReceiveAttack(combatInfo, transform.position.x);
            readyToAttack = false;
        }
    }

    public void ReceiveAttack(CombatInfo combatInfo, float enemyXPosition)
    {
        playerInfo.CurrentHealthPoint -= combatInfo.damage;
        if (combatInfo.isKnockDown)
        {
            playerInfo.CurrentKnockDownPoint = 0f;
            playerController.Damaged(combatInfo.applyVelocity, combatInfo.stiffTime, true, enemyXPosition);
        }
        else if (playerInfo.IsKnockDown)
        {
            playerInfo.CurrentKnockDownPoint = 0f;
            playerController.Damaged(combatInfo.applyVelocity, 0, true, enemyXPosition);
        }
        else
        {
            playerController.Damaged(combatInfo.applyVelocity, combatInfo.stiffTime, false, enemyXPosition);
        }
    }

    public void Fallout()
    {
        Debug.LogFormat("Player {0} Fallout.", gameObject.name);
        playerInfo.CurrentHealthPoint -= 20;
        playerInfo.CurrentKnockDownPoint = 0f;
        playerController.Fallout();
    }

    public void PrepareAttack(CombatInfo info)
    {
        readyToAttack = true;
        combatInfo = info;
    }

    public void CancelAttack()
    {
        readyToAttack = false;
    }
}
