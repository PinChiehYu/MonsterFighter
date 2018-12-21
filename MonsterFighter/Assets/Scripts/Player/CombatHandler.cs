using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo), typeof(PlayerController))]
public class CombatHandler : MonoBehaviour, ICombatSender, IBorderSensor {

    private PlayerInfo playerInfo;
    private PlayerController playerController;

    private int comboCounter;

    private bool readyToAttack;
    private CombatInfo combatInfo;

    public bool Invincible { get; set; }
    public event Action OnHitTarget;
    public event Action OnReceiveCrit;
    public event Action OnReceiveAttack;

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        playerController = GetComponent<PlayerController>();
        comboCounter = 0;
        readyToAttack = false;
        Invincible = false;
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
        if (Invincible) return;
        OnReceiveAttack?.Invoke();

        GetComponent<AudioSource>().clip = combatInfo.hitClip;
        GetComponent<AudioSource>().Play();

        playerInfo.CurrentHealthPoint -= combatInfo.damage;

        if (combatInfo.isCrit)
        {
            OnReceiveCrit?.Invoke();
        }

        if (combatInfo.stateType == StateType.Combat)
        {
            playerInfo.CurrentKnockdownPoint += combatInfo.damage;
        }

        if (combatInfo.isKnockDown || playerInfo.IsDead)
        {
            playerInfo.CurrentKnockdownPoint = 0f;
            playerController.Damaged(combatInfo.applyVelocity, combatInfo.stiffTime, true, enemyXPosition);
        }
        else if (playerInfo.IsKnockdown)
        {
            playerInfo.CurrentKnockdownPoint = 0f;
            playerController.Damaged(combatInfo.applyVelocity, 0, true, enemyXPosition);
        }
        else
        {
            playerController.Damaged(combatInfo.applyVelocity, combatInfo.stiffTime, false, enemyXPosition);
        }
    }

    public void Fallout()
    {
        OnReceiveAttack?.Invoke();
        playerInfo.CurrentHealthPoint -= 100;
        playerInfo.CurrentKnockdownPoint = 0f;
        playerController.Fallout();
        OnReceiveCrit?.Invoke();
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
