using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MapSelectSheet : MonoBehaviour
{
    [SerializeField]
    private List<MapSelectInfo> selectionList;

    private KeyCode leftInput, rightInput, selectInput;
    private int mapPointer;

    private Image mapImage;
    private Image mapName;
    private Image lockImage;

    private AudioSource clickAudio;
    [SerializeField]
    private AudioClip switchClip, lockClip;

    private SelectManager selectManager;

    void Awake()
    {
        mapImage = transform.Find("MapImage").GetComponent<Image>();
        mapName = transform.Find("MapName").GetComponent<Image>();
        lockImage = transform.Find("Lock").GetComponent<Image>();
        selectManager = FindObjectOfType<SelectManager>();
        clickAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        leftInput = GameManager.Instance.playerControlSets[0]["Left"];
        rightInput = GameManager.Instance.playerControlSets[0]["Right"];
        selectInput = GameManager.Instance.playerControlSets[0]["AtkL"];

        clickAudio.clip = switchClip;
        mapPointer = 0;
        SelectMap(4);
    }

    private void Update()
    {
        if (mapPointer < 0) return;
        if (Input.GetKeyDown(leftInput) && mapPointer - 1 >= 0)
        {
            mapPointer--;
            SelectMap(mapPointer + 1);
            clickAudio.Play();
        }
        else if (Input.GetKeyDown(rightInput) && mapPointer + 1 < selectionList.Count)
        {
            mapPointer++;
            SelectMap(mapPointer - 1);
            clickAudio.Play();
        }
        else if (Input.GetKeyDown(selectInput))
        {
            clickAudio.clip = lockClip;
            clickAudio.Play();
            FinishSelect();
        }
    }

    private void FinishSelect()
    {
        foreach (MapSelectInfo button in selectionList)
        {
            button.gameObject.SetActive(false);
        }
        lockImage.gameObject.SetActive(true);
        mapPointer = -1;

        selectManager.StartBattle();
    }

    private void SelectMap(int prePointer)
    {
        mapImage.sprite = selectionList[mapPointer].mapSprite;
        mapName.sprite = selectionList[mapPointer].mapNameSprite;
        selectionList[prePointer].GetComponent<Image>().color = Color.white;
        selectionList[mapPointer].GetComponent<Image>().color = Color.red;

        selectManager.PickMap(selectionList[mapPointer].mapName);
    }
}
