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

    private PlayerController playerController;

    private int currentHealthPoind;
    public float CurrentHealthPoint {
        get { return currentHealthPoind; }
        set {
            currentHealthPoind = Mathf.Clamp((int)value, 0, 100);
            if (OnHPChange != null)
            {
                OnHPChange.Invoke((float)currentHealthPoind / baseManaPoint);
            }
            if (currentHealthPoind == 0 && OnDie != null)
            {
                OnDie.Invoke(id);
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
        playerController = GetComponent<PlayerController>();
    }

    public void InitPlayer()
    {
        actionState = ActionState.Normal;
        combatState = CombatState.Transition;
        movementState = MovementState.Normal;
        CurrentHealthPoint = baseHealthPoint;
        CurrentManaPoint = 0;
        CurrentEnergyPoint = 0;

        playerController.InitController();
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
