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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendAttack(CombatHandler enemyHandler)
    {
        if (readyToAttack)
        {
            OnHitTarget?.Invoke();
            enemyHandler.ReceiveAttack(currentCombatInfo, transform.position.x);
            readyToAttack = false;
        }
    }

    public void ReceiveAttack(CombatInfo combatInfo, float enemyXPosition)
    {
        Debug.LogFormat("Player {0} Get {1} Points Hit.", playerInfo.id, combatInfo.damage);
        playerInfo.CurrentHealthPoint -= combatInfo.damage;
        playerController.Damaged(enemyXPosition);
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
}
