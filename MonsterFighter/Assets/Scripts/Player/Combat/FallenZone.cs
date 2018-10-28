using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenZone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.LogFormat("Character Fall! ParentName:{0}", collider.gameObject.transform.parent.name);
        collider.GetComponentInParent<CombatHandler>().Fallout();
    }
}
