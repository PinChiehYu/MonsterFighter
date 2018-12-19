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
    private int skillSCost;
    [SerializeField]
    private int skillBCost;

    private int currentHealthPoint;
    public float CurrentHealthPoint
    {
        get { return currentHealthPoint; }
        set
        {
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

    private int currentKnockdownPoint;
    public float CurrentKnockDownPoint
    {
        get { return currentKnockdownPoint * baseHealthPoint; }
        set
        {
            currentKnockdownPoint = Mathf.Clamp((int)(value / baseHealthPoint), 0, 30);
            OnKnockdownChange?.Invoke(currentKnockdownPoint);
        }
    }
    public bool IsKnockdown { get { return currentKnockdownPoint >= 30; } }

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
    public event Action<float> OnKnockdownChange;
    public event Action<int> OnDie;

    public void ResetPlayerInfo()
    {
        CurrentHealthPoint = baseHealthPoint;
        CurrentManaPoint = 0;
        CurrentKnockDownPoint = 0;
    }

    private IEnumerator declineCor;
    private void RestartKnockdownTiming()
    {
        if (declineCor != null)
        {
            StopCoroutine(declineCor);
        }
        declineCor = KnockdownValueDecline();
        StartCoroutine(declineCor);
    }

    IEnumerator KnockdownValueDecline()
    {
        yield return new WaitForSeconds(3f);
        while (CurrentKnockDownPoint > 0f)
        {
            CurrentKnockDownPoint -= 0.06f * baseHealthPoint;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
