using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CharSelectSheet : MonoBehaviour
{
    [SerializeField]
    private int playerId;
    [SerializeField]
    private List<CharSelectInfo> selectionList;

    private KeyCode leftInput, rightInput, selectInput;
    private int characterPointer;

    private Image characterImage;
    private Image characterName;
    private Image lockImage;

    private SelectManager selectManager;

    void Awake()
    {
        characterImage = transform.Find("CharacterImage").GetComponent<Image>();
        characterName = transform.Find("CharacterName").GetComponent<Image>();
        lockImage = transform.Find("Lock").GetComponent<Image>();
        selectManager = FindObjectOfType<SelectManager>();

        leftInput = GameManager.Instance.playerControlSets[playerId]["Left"];
        rightInput = GameManager.Instance.playerControlSets[playerId]["Right"];
        selectInput = GameManager.Instance.playerControlSets[playerId]["AtkL"];
        characterPointer = 0;
        SelectCharacter(0);
    }

    private void Update()
    {
        if (characterPointer < 0) return;
        if (Input.GetKeyDown(leftInput) && characterPointer - 1 >= 0)
        {
            characterPointer--;
            SelectCharacter(characterPointer + 1);
        }
        else if (Input.GetKeyDown(rightInput) && characterPointer + 1 < selectionList.Count)
        {
            characterPointer++;
            SelectCharacter(characterPointer - 1);
        }
        else if (Input.GetKeyDown(selectInput))
        {
            FinishSelect();
        }
    }

    private void FinishSelect()
    {
        foreach (CharSelectInfo button in selectionList)
        {
            button.gameObject.SetActive(false);
        }
        lockImage.gameObject.SetActive(true);
        characterPointer = -1;

        FindObjectOfType<SelectManager>().FinishSelect(playerId);
    }

    private void SelectCharacter(int prePointer)
    {
        characterImage.sprite = selectionList[characterPointer].characterSprite;
        characterName.sprite = selectionList[characterPointer].characterNameSprite;
        selectionList[prePointer].GetComponent<Image>().color = Color.white;
        selectionList[characterPointer].GetComponent<Image>().color = Color.red;

        GameManager.Instance.playerCharacterPicks[playerId] = selectionList[characterPointer].characterName;
    }
}
