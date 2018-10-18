using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    // Use this for initialization
    private Animator animator;
	void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Damaged");
        }
	}
}
