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
    private Tuple<float, Vector2> combatInfo;

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
            enemyHandler.ReceiveAttack(combatInfo.Item1, combatInfo.Item2, transform.position.x);
            readyToAttack = false;
        }
    }

    public void ReceiveAttack(float damage, Vector2 applyVelocity, float enemyXPosition)
    {
        playerInfo.CurrentHealthPoint -= damage;
        playerController.Damaged(applyVelocity, enemyXPosition);
    }

    public void Fallout()
    {
        Debug.LogFormat("Player {0} Fallout.", gameObject.name);
        playerInfo.CurrentHealthPoint -= 20;
        playerController.Fallout();
    }

    public void PrepareAttack(float damage, Vector2 applyVelocity)
    {
        readyToAttack = true;
        combatInfo = new Tuple<float, Vector2>(damage, applyVelocity);
    }

    public void CancelAttack()
    {
        readyToAttack = false;
    }
}
