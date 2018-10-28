using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo), typeof(PlayerController))]
public class CombatHandler : MonoBehaviour {

    private PlayerInfo playerInfo;
    private PlayerController playerController;

    private bool readyToAttack;

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        playerController = GetComponent<PlayerController>();
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
            enemyHandler.ReceiveAttack(10, transform.position.x);
            readyToAttack = false;
        }
    }

    public void ReceiveAttack(int damage, float enemyXPosition)
    {
        //Debug.LogFormat("Player {0} Get {1} Points Hit.", playerInfo.id, damage);
        playerInfo.CurrentHealthPoint -= damage;
        playerController.Damaged(enemyXPosition);
    }

    public void Fallout()
    {
        Debug.LogFormat("Player {0} Fallout.", playerInfo.id);
        playerInfo.CurrentHealthPoint -= 20;
        playerController.Fallout();
    }

    public void PrepareAttack()
    {
        readyToAttack = true;
    }
}
