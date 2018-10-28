using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionInfo {
    public int id;
    public ActionType actionType;
    public float triggerFrame;
    public int actionValue;
    public float startNextInputTime;
}
