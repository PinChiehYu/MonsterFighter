using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatSender {
    void SendAttack(CombatHandler combatHandler);
}
