using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackboxDetector : MonoBehaviour {

    public int id { get; set; }
    private CombatHandler combatHandler;    

    void Awake()
    {
        combatHandler = GetComponentInParent<CombatHandler>();
        id = GetComponentInParent<PlayerInfo>().id;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponentInParent<PlayerInfo>().id != id )
        {
            combatHandler.SendAttack(collider.GetComponentInParent<CombatHandler>());
        }
    }
}
