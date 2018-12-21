﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour {

    // Use this for initialization
    private GameObject charCanvas, mapCanvas;
    private string mapName;

    void Awake ()
    {
        charCanvas = GameObject.Find("CharacterCanvas");
        mapCanvas = GameObject.Find("MapCanvas");
    }

    void Start()
    {
        mapCanvas.SetActive(false);

        if (GameManager.Instance.gameMode == GameMode.Practice)
        {
            selected[1] = true;
            GameObject.Find("P2Character").SetActive(false);
        }
    }

    private bool[] selected = new bool[2];
    public void FinishSelect(int playerid)
    {
        selected[playerid] = true;

        if(selected[0] && selected[1])
        {
            StartCoroutine(SwitchSelectionCanvas());
        }
    }

    public void PickMap(string map)
    {
        mapName = map;
        Debug.Log(mapName);
    }

    public void StartBattle()
    {
        mapName = GameManager.Instance.gameMode == GameMode.Practice ? "Practice" : mapName;
        SceneManager.LoadScene(mapName);
    }

    IEnumerator SwitchSelectionCanvas()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance.gameMode == GameMode.Practice)
        {
            StartBattle();
        }
        else
        {
            charCanvas.SetActive(false);
            mapCanvas.SetActive(true);
        }
    }
}
