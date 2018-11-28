using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public int id;// { get; set; }

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
            Debug.LogFormat("Player {0} remain HP:{1}", id, (float)currentHealthPoint / baseHealthPoint);
            OnHPChange?.Invoke((float)currentHealthPoint / baseHealthPoint);
            if (currentHealthPoint <= 0)
            {
                OnDie?.Invoke(id);
            }
        }
    }
    private int currentManaPoint;
    public float CurrentManaPoint { get { return currentManaPoint / baseManaPoint; } set { currentManaPoint = (int)value; } }
    private int currentEnergyPoint;
    public float CurrentEnergyPoint;

    public ActionState actionState { get; private set; }
    public CombatState combatState { get; private set; }
    public MovementState movementState { get; private set; }

    public event Action<float> OnHPChange;
    public event Action OnMPChange;
    public event Action OnEPChange;

    public event Action<int> OnDie;

    void Awake()
    {

    }

    public void InitPlayer()
    {
        actionState = ActionState.Normal;
        combatState = CombatState.Transition;
        movementState = MovementState.Normal;
        CurrentHealthPoint = baseHealthPoint;
        CurrentManaPoint = 0;
        CurrentEnergyPoint = 0;

        GetComponent<PlayerController>().InitController();
        GetComponent<PhysicsObject>().InitPhysics();
    }

    public void SwitchActionState(ActionState aS)
    {
        actionState = aS;
    }

    public void SwitchMovementState(MovementState mS)
    {
        movementState = mS;
    }

    public void SwitchCombatState(CombatState cS)
    {
        combatState = cS;
    }
}
