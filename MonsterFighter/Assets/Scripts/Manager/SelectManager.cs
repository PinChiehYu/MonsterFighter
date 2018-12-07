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
        mapCanvas.SetActive(false);
        mapName = "";
    }

    private bool[] selected = new bool[2];
    public void FinishSelect(int playerid)
    {
        selected[playerid] = true;

        if(selected[0] && selected[1])
        {
            charCanvas.SetActive(false);
            mapCanvas.SetActive(true);
        }
    }

    public void PickMap(string map)
    {
        if (mapName == map)
        {
            StartBattle();
        }

        mapName = map;
    }

    public void StartBattle()
    {
        SceneManager.LoadScene(mapName); //GameManager.Instance.gameMode
    }
}