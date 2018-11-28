using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Combo : MonoBehaviour {

    // Use this for initialization
    int comboCount;
    int countdown;
    float timer;
    private TMP_Text Text;

    void Start () {
        Text = GetComponent<TMP_Text>();
        comboCount = 0;
        countdown = 3;
        timer = countdown;
    }

    public void OnPlayerHpChange(float p)
    {
        timer = countdown;
        comboCount++;
        transform.DOMoveX(10f, countdown);
    }

    // Update is called once per frame
    void Update () {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            comboCount = 0;
        }
    }
}
