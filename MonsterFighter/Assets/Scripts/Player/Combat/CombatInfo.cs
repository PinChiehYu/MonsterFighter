using UnityEngine;

[System.Serializable]
public class CombatInfo
{
    public StateType stateType;
    public int damage;
    public Vector2 applyVelocity;
    public bool isKnockDown;
    public float stiffTime;
    public bool isCrit;
    public AudioClip hitClip;

    public CombatInfo(StateType type, int damage, Vector2 applyvelocity, bool isknockdown, float stifftime, bool iscrit, AudioClip hitclip)
    {
        stateType = type;
        this.damage = damage;
        applyVelocity = applyvelocity;
        isKnockDown = isknockdown;
        stiffTime = stifftime;
        isCrit = iscrit;
        hitClip = hitclip;
    }
}
