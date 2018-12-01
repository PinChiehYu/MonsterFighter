using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatInfo {
    public CombatType combatType;
    public float triggerFrame;
    [Space(25)]
    public float damage;
    public Vector2 applyVelocity;
    [Space(25)]
    public GameObject projectile;
    public Vector2 launchPoint;

    public void Execute(CombatHandler handler)
    {
        if(combatType == CombatType.Attack)
        {
            handler.PrepareAttack(damage, applyVelocity);
        }
        else if (combatType == CombatType.Project)
        {
            Vector3 point = new Vector3(launchPoint.x, launchPoint.y, 0f);
            int flip = handler.transform.rotation.eulerAngles.y > 90f  ? -1 : 1;
            point.x *= flip;
            Projectile instance = Object.Instantiate(projectile, handler.transform.position + point, handler.transform.rotation).GetComponent<Projectile>();
            instance.ownerId = handler.gameObject.name;
            instance.damage = damage;
            instance.applyVelocity = applyVelocity;
        }
    }
}
