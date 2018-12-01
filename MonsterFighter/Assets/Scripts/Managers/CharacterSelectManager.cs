using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour {

    private List<string> characterList;

	// Use this for initialization
    void Awake ()
    {
        characterList = new List<string>();
        characterList.Add("Coco");
        GameManager.Instance.playerCharacterPicks[0] = "Coco";
        GameManager.Instance.playerCharacterPicks[1] = "Coco";
    }

    public void SelectCharacter(string charName)
    {
        GameManager.Instance.playerCharacterPicks[1] = charName;
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("Battle"); //GameManager.Instance.gameMode
    }
}
