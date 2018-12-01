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

    private int currentHealthPoint;
    public float CurrentHealthPoint {
        get { return currentHealthPoint; }
        set {
            currentHealthPoint = (int)value;
            OnHPChange?.Invoke((float)currentHealthPoint / baseHealthPoint);
            if (currentHealthPoint <= 0)
            {
                OnDie?.Invoke(int.Parse(gameObject.name));
            }
        }
    }
    private int currentManaPoint;
    public float CurrentManaPoint { get { return currentManaPoint / baseManaPoint; } set { currentManaPoint = (int)value; } }
    private int currentEnergyPoint;
    public float CurrentEnergyPoint;

    public event Action<float> OnHPChange;
    public event Action OnMPChange;
    public event Action OnEPChange;

    public event Action<int> OnDie;

    void Awake()
    {

    }

    public void ResetPlayer()
    {
        CurrentHealthPoint = baseHealthPoint;
        CurrentManaPoint = 0;
        CurrentEnergyPoint = 0;

        GetComponent<PlayerController>().ResetController();
        GetComponent<PhysicsObject>().ResetPhysics();
    }
}
