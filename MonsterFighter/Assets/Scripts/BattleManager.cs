using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    // Use this for initialization
    private GameObject[] playerCharacters = new GameObject[2];


    void Awake ()
    {
        InstantiateCharacters();
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InstantiateCharacters()
    {
        playerCharacters[0] = GameManager.Instance.CreateCharacter(0);
        playerCharacters[1] = GameManager.Instance.CreateCharacter(1);

        Debug.Log(playerCharacters[0].name);
    }
}
