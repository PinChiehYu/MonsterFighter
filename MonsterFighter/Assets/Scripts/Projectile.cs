using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Projectile : MonoBehaviour, ICombatSender {
    // Use this for initialization
    public string ownerId;
    public float speed;
    public float damage;
    public Vector2 applyVelocity;

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
        combatHandler.ReceiveAttack(damage, applyVelocity, transform.position.x);
        gameObject.SetActive(false);
        OnHit();
    }

    public void End()
    {
        Destroy(gameObject);
    }

    private void OnHit()
    {
        GetComponent<Animator>().SetTrigger("Hit");
        Debug.Log("Missile Hit!");
    }
}
