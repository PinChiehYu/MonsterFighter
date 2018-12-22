using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Projectile : MonoBehaviour, ICombatSender, IBorderSensor {
    [HideInInspector]
    public string ownerId;
    public float speed;
    public bool isPenetrant;
    [HideInInspector]
    public CombatInfo combatInfo;

    void Start()
    {
        GetComponentInChildren<AttackboxDetector>().Id = ownerId;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void SendAttack(CombatHandler combatHandler)
    {
        combatHandler.ReceiveAttack(combatInfo, transform.position.x);
        OnHit();
        if (isPenetrant)
        {
            transform.Find("Attack").gameObject.SetActive(false);
        }
        else
        {
            Ending();
        }
    }

    public void Ending()
    {
        Destroy(gameObject);
    }

    private void OnHit()
    {
        GetComponent<Animator>().SetTrigger("Hit");
    }

    public void Fallout()
    {
        Ending();
    }
}
