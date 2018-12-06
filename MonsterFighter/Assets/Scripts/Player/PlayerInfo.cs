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
    private int skillSCost;
    [SerializeField]
    private int skillBCost;

    private int currentHealthPoint;
    public float CurrentHealthPoint
    {
        get { return currentHealthPoint; }
        set
        {
            CurrentKnockDownPoint += (currentHealthPoint - (int)value) * 100f / baseHealthPoint;
            currentHealthPoint = Mathf.RoundToInt(value);
            OnHpChange?.Invoke((float)currentHealthPoint / baseHealthPoint);
            RestartKnockdownTiming();
            if (currentHealthPoint <= 0)
            {
                OnDie?.Invoke(int.Parse(gameObject.name));
            }
        }
    }
    private float currentManaPoint;
    public float CurrentManaPoint
    {
        get { return currentManaPoint; }
        set
        {
            currentManaPoint = Mathf.Clamp(value, 0, 100);
            OnMpChange?.Invoke(currentManaPoint / baseManaPoint);
        }
    }
    private int currentEnergyPoint;
    public float CurrentEnergyPoint { get { return currentEnergyPoint / baseEnergyPoint; } set { currentEnergyPoint = (int)value; } }

    private float currentKnockdownPoint;
    public float CurrentKnockDownPoint
    {
        get { return currentKnockdownPoint; }
        set
        {
            currentKnockdownPoint = Mathf.Clamp(value, 0f, 30f);
        }
    }
    public bool IsKnockdown { get { return currentKnockdownPoint >= 30f; } }

    public bool AbleToCastSkillS
    {
        get
        {
            if(currentManaPoint >= skillSCost)
            {
                CurrentManaPoint -= skillSCost;
                return true;
            }
            return false;
        }
    }
    public bool AbleToCastSkillB
    {
        get
        {
            if (currentManaPoint >= skillBCost)
            {
                CurrentManaPoint -= skillBCost;
                return true;
            }
            return false;
        }
    }

    public event Action<float> OnHpChange;
    public event Action<float> OnMpChange;
    public event Action<float> OnEPChange;
    public event Action<int> OnDie;

    public void ResetPlayerInfo()
    {
        CurrentHealthPoint = baseHealthPoint;
        CurrentManaPoint = 100;
        CurrentEnergyPoint = 0;
    }

    private IEnumerator declineCo;
    private void RestartKnockdownTiming()
    {
        if (declineCo != null)
        {
            StopCoroutine(declineCo);
        }
        declineCo = KnockdownValueDecline();
        StartCoroutine(declineCo);
    }

    IEnumerator KnockdownValueDecline()
    {
        yield return new WaitForSeconds(3f);
        while (CurrentKnockDownPoint > 0f)
        {
            CurrentKnockDownPoint -= 0.06f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
