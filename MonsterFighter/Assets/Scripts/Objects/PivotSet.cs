using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSet : MonoBehaviour {
    public List<Vector3> GetPivotsPosition()
    {
        List<Vector3> positionlist = new List<Vector3>();
        positionlist.Add(transform.Find("Start_P0").transform.position);
        positionlist.Add(transform.Find("Fall_P0").transform.position);
        positionlist.Add(transform.Find("Start_P1").transform.position);
        positionlist.Add(transform.Find("Fall_P1").transform.position);

        return positionlist;
    }
}
