using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    [SerializeField]
    private int baseHealthPoint;
    [SerializeField]
    private int baseManaPoint;
    [SerializeField]
    private int baseEnergyPoint;
    [SerializeField]
    private int SkillSCost;
    [SerializeField]
    private int SkillBCost;

    private int currentHealthPoint;
    public float CurrentHealthPoint
    {
        get { return currentHealthPoint; }
        set {
            CurrentKnockDownPoint += (currentHealthPoint - (int)value) * 100f / baseHealthPoint;
            currentHealthPoint = (int)value;
            OnHPChange?.Invoke((float)currentHealthPoint / baseHealthPoint);
            RestartKnockDownTiming();
            if (currentHealthPoint <= 0)
            {
                OnDie?.Invoke(int.Parse(gameObject.name));
            }
        }
    }
    private int currentManaPoint;
    public float CurrentManaPoint { get { return currentManaPoint / baseManaPoint; } set { currentManaPoint = (int)value; } }
    private int currentEnergyPoint;
    public float CurrentEnergyPoint { get { return currentEnergyPoint / baseEnergyPoint; } set { currentEnergyPoint = (int)value; } }

    private float currentKnockDownPoint;
    public float CurrentKnockDownPoint
    {
        get { return currentKnockDownPoint; }
        set
        {
            currentKnockDownPoint = Mathf.Clamp(value, 0f, 30f);
        }
    }
    public bool IsKnockDown { get { return currentKnockDownPoint >= 30f; } }

    public event Action<float> OnHPChange;
    public event Action<float> OnMPChange;
    public event Action<float> OnEPChange;
    public event Action<int> OnDie;

    public void ResetPlayer()
    {
        CurrentHealthPoint = baseHealthPoint;
        CurrentManaPoint = 0;
        CurrentEnergyPoint = 0;

        GetComponent<PlayerController>().ResetController();
        GetComponent<PhysicsObject>().ResetPhysics();
    }

    private IEnumerator declineCo;
    private void RestartKnockDownTiming()
    {
        if (declineCo != null)
        {
            StopCoroutine(declineCo);
        }
        declineCo = KnockDownValueDecline();
        StartCoroutine(declineCo);
    }

    IEnumerator KnockDownValueDecline()
    {
        yield return new WaitForSeconds(3f);
        while (CurrentKnockDownPoint > 0f)
        {
            CurrentKnockDownPoint -= 0.06f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
