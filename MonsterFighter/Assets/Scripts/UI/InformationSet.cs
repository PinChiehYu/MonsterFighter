﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationSet : MonoBehaviour {

    [SerializeField]
    private int playerId;

    private Image HPBar;
    private Image MPBar;
    private Image[] Lights = new Image[2];

    //private Image EPBar;
    //private BattleManager battleManager;

    private float newHpPercentage, newMpPercentage;

    void Awake ()
    {
        HPBar = transform.GetChild(0).GetComponent<Image>();
        MPBar = transform.GetChild(1).GetComponent<Image>();
        Lights[0] = transform.GetChild(2).GetComponent<Image>();
        Lights[1] = transform.GetChild(3).GetComponent<Image>();
        newHpPercentage = 1f;
        newMpPercentage = 1f;
    }
	
	// Update is called once per frame
	void Update () {
        HPBar.fillAmount = Mathf.Lerp(HPBar.fillAmount, newHpPercentage, 0.1f);
        MPBar.fillAmount = Mathf.Lerp(MPBar.fillAmount, newMpPercentage, 0.1f);
    }

    public void OnPlayerHpChange(float percentage)
    {
        newHpPercentage = percentage;
    }

    public void LightBubble(int lightId)
    {
        StartCoroutine("DisplayLight", lightId - 1);
    }

    IEnumerator DisplayLight(int id)
    {
        while(Lights[id].fillAmount < 1)
        {
            Lights[id].fillAmount = Mathf.Lerp(Lights[id].fillAmount, 1, 0.05f);
            yield return null;
        }
    }
}
