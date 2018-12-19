using UnityEngine;

[System.Serializable]
public class CombatInfo
{
    public int damage;
    public Vector2 applyVelocity;
    public bool isKnockDown;
    public float stiffTime;
    public bool isCrit;
    public AudioClip hitClip;

    public CombatInfo(int damage, Vector2 applyvelocity, bool isknockdown, float stifftime, bool iscrit, AudioClip hitclip)
    {
        this.damage = damage;
        applyVelocity = applyvelocity;
        isKnockDown = isknockdown;
        stiffTime = stifftime;
        isCrit = iscrit;
        hitClip = hitclip;
    }
}
