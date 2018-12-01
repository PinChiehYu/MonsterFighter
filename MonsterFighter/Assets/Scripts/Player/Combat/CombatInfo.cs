using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatInfo {
    public CombatType combatType;
    public float triggerFrame;
    public int damage;
    public Vector2 applyVelocity;
}
