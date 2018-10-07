using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour {

	// Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectCharacter(Button button)
    {
        GameManager.Instance.playerCharacterPicks[0] = button.name;
        SceneManager.LoadScene("Battle");
    }
}
