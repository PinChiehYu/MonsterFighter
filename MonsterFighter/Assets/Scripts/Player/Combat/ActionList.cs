using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewActionList", menuName = "ActionList")]
public class ActionList : ScriptableObject {

    private int currentInfoId;
    [SerializeField]
    private List<ActionInfo> actionList;
    [HideInInspector]
    public ActionInfo currentInfo { get { return actionList[currentInfoId]; } }

}
