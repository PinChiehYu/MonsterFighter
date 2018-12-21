using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponentInParent<IBorderSensor>().Fallout();
    }
}
