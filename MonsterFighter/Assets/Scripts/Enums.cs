public enum ActionState
{
    Normal,
    Action
}

public enum MovementState
{
    Normal,
    Immovable
}

public enum CombatState
{
    ReceiveInput,
    Transition
}

public enum ActionType
{
    Attack,
    Buff
}

public enum AtkTrigger
{
    None,
    AtkL,
    AtkH
}
