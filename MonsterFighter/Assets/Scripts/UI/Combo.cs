using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour {

    // Use this for initialization
    int comboCount;
    int countdown;
    float timer;
    private TMP_Text Text;

    private Vector2 basePoint;

    void Awake () {
        Text = GetComponent<TMP_Text>();
        comboCount = 0;
        countdown = 3;
        timer = countdown;
        basePoint = GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(basePoint);
    }

    public void OnPlayerHpChange(float p)
    {
        timer = countdown;
        comboCount++;
        Debug.Log(comboCount);
        //GetComponent<RectTransform>().anchoredPosition = basePoint;
        //transform.DOMoveX(10f, countdown);
        GetComponent<TMP_Text>().text = comboCount.ToString();
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
