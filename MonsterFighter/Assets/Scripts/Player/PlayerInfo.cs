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
            currentHealthPoint = Mathf.Clamp((int)value, 0, baseHealthPoint);
            OnHpChange?.Invoke((float)currentHealthPoint / baseHealthPoint);
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

    private float currentKnockdownPoint;
    public float CurrentKnockdownPoint
    {
        get { return currentKnockdownPoint * baseHealthPoint / 100; }
        set
        {
            RestartKnockdownTiming();
            currentKnockdownPoint = Mathf.Clamp((value * 100 / baseHealthPoint), 0f, 30f);
            OnKnockdownChange?.Invoke(currentKnockdownPoint);
        }
    }
    public bool IsKnockdown { get { return currentKnockdownPoint >= 30f; } }

    public bool IsDead { get { return currentHealthPoint <= 0; } }

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
        CurrentKnockdownPoint = 0;
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

    private IEnumerator KnockdownValueDecline()
    {
        yield return new WaitForSeconds(3f);
        while (currentHealthPoint > 0f)
        {
            currentKnockdownPoint = Mathf.Clamp(currentKnockdownPoint - 0.3f, 0f, 30f);
            OnKnockdownChange?.Invoke(currentKnockdownPoint);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
