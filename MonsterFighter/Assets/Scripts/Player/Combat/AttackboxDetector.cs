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
        string targetname = collider.transform.parent.name;
        if (targetname != Id && targetname.Length == 1)
        {
            combatSender.SendAttack(collider.GetComponentInParent<CombatHandler>());
        }
    }
}
