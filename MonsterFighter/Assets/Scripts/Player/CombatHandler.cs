using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo), typeof(PlayerController))]
public class CombatHandler : MonoBehaviour {

    private PlayerInfo playerInfo;
    private PlayerController playerController;

    private int comboCounter;

    private bool readyToAttack;
    private CombatInfo currentCombatInfo;

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
            enemyHandler.ReceiveAttack(currentCombatInfo, transform.position.x);
            readyToAttack = false;
        }
    }

    public void ReceiveAttack(CombatInfo combatInfo, float enemyXPosition)
    {
        Debug.LogFormat("Player {0} Get {1} Points Hit.", playerInfo.id, combatInfo.damage);
        Debug.LogFormat("Player {0} Move {1}.", playerInfo.id, combatInfo.applyVelocity);
        playerInfo.CurrentHealthPoint -= combatInfo.damage;
        playerController.Damaged(combatInfo.damage, combatInfo.applyVelocity, enemyXPosition);
    }

    public void Fallout()
    {
        Debug.LogFormat("Player {0} Fallout.", playerInfo.id);
        playerInfo.CurrentHealthPoint -= 20;
        playerController.Fallout();
    }

    public void PrepareAttack(CombatInfo combatInfo)
    {
        readyToAttack = true;
        currentCombatInfo = combatInfo;
    }

    public void CancelAttack()
    {
        readyToAttack = false;
    }
}
