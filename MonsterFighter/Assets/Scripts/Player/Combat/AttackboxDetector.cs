using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackboxDetector : MonoBehaviour {

    public string Id { get; set; }
    private ICombatSender combatSender;

    void Awake()
    {
        combatSender = GetComponentInParent<ICombatSender>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(collider.transform.parent.name + ":" + Id);
        if (collider.transform.parent.name != Id )
        {
            combatSender.SendAttack(collider.GetComponentInParent<CombatHandler>());
        }
    }
}
