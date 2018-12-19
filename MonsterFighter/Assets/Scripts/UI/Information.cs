using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Information : MonoBehaviour {

    [SerializeField]
    private List<Sprite> iconList;

    private StatusBar[] statusBars = new StatusBar[2];

    private TMP_Text announce;
    private TMP_Text timer;

    void Awake()
    {
        announce = transform.Find("Announce").GetComponent<TMP_Text>();
        announce.gameObject.SetActive(false);
        timer = transform.Find("Timer").GetComponent<TMP_Text>();
        statusBars[0] = transform.Find("StatusBar_0").GetComponent<StatusBar>();
        statusBars[1] = transform.Find("StatusBar_1").GetComponent<StatusBar>();

        statusBars[0].SetIcon(iconList[GameManager.Instance.GetPlayerPickId(0)]);
        statusBars[1].SetIcon(iconList[GameManager.Instance.GetPlayerPickId(1)]);
    }

    public Action<float> OnPlayerHpChange(int playerId)
    {
        return statusBars[playerId].OnPlayerHpChange;
    }

    public Action<float> OnPlayerMpChange(int playerId)
    {
        return statusBars[playerId].OnPlayerMpChange;
    }

    public Action<float> OnPlayerKnockdownChange(int playerId)
    {
        return statusBars[playerId].OnPlayerKnockdownChange;
    }

    public void UpdateTimer(int time)
    {
        timer.text = time.ToString();
    }

    public void TurnOnAnnounce(string announcement)
    {
        announce.gameObject.SetActive(true);
        announce.text = announcement;
    }

    public void TurnOffAnnounce()
    {
        announce.gameObject.SetActive(false);
    }

    public void LightUpBubble(int playerId, int winCount)
    {
        statusBars[playerId].LightBubble(winCount);
    }
}
