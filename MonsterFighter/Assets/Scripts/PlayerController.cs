using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    private int playerId;
    private Dictionary<string, KeyCode> controlSet = new Dictionary<string, KeyCode>();
    private Queue<string> InputBuffer = new Queue<string>();

    private Animator animator;
    
	void Awake () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(controlSet["Up"]))
        {
            //animator.settr
            animator.SetTrigger("Up");
        }
        else if (Input.GetKeyDown(controlSet["Down"]))
        {
            animator.SetTrigger("Down");
        }
        else if (Input.GetKeyDown(controlSet["Right"]))
        {
            
            animator.SetTrigger("Right");
        }
        else if (Input.GetKeyDown(controlSet["Left"]))
        {
            animator.SetTrigger("Left");
        }
        else if (Input.GetKeyDown(controlSet["Attack1"]))
        {
            animator.SetTrigger("Attack1");
        }
        else if (Input.GetKeyDown(controlSet["Attack2"]))
        {
            animator.SetTrigger("Attack2");
        }
    }

    private void ResetAllAnimatorTrigger()
    {
        //animator.
    }

    public void SetPlayerId(int id)
    {
        playerId = id;
        if (playerId == 1) transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void SetControlSet(Dictionary<string, KeyCode> controlset)
    {
        controlSet = controlset;
    }
}
