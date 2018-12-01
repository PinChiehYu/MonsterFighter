using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class BattleManager : MonoBehaviour {

    [SerializeField]
    private int maxWinRound;
    private int maxTimePerRound;

    private int roundCounter;
    private float countdownTimer;

    private TMP_Text timerText;
    private TMP_Text announceText;
    private InformationSet[] informationSets;
    private Combo[] comboSets;

    private int[] playerComboHit = new int[2] { 0, 0 };

    private PlayerInfo[] playerInfos = new PlayerInfo[2];
    private int[] playerWinCount = new int[2] { 0, 0 };

    void Awake ()
    {
        maxTimePerRound = 90;
        roundCounter = 1;
       
        timerText = GameObject.Find("CountDownTimer").GetComponent<TMP_Text>();
        announceText = GameObject.Find("Announce").GetComponent<TMP_Text>();
        announceText.gameObject.SetActive(false);
        informationSets = GameObject.Find("Canvas").GetComponentsInChildren<InformationSet>();
        comboSets = GameObject.Find("Canvas").GetComponentsInChildren<Combo>();

        InstantiateCharacters();
        RegisterEvent();
        StartNewRound();
    }

    private void InstantiateCharacters()
    {
        playerInfos[0] = GameManager.Instance.CreateCharacter(0).GetComponent<PlayerInfo>();
        playerInfos[1] = GameManager.Instance.CreateCharacter(1).GetComponent<PlayerInfo>();
    }

    private void RegisterEvent()
    {
        for (int i = 0; i < 2; i++)
        {
            playerInfos[i].OnDie += CharacterDie;
            playerInfos[i].OnHPChange += informationSets[i].OnPlayerHpChange;
            playerInfos[i].OnHPChange += comboSets[i].OnPlayerHpChange;
        }
    }

    private void StartNewRound()
    {
        countdownTimer = maxTimePerRound;
        ResetPlayers();
        StartCoroutine(StartRoundDisplay(roundCounter));
    }

    private void ResetPlayers()
    {
        playerInfos[0].ResetPlayer();
        playerInfos[1].ResetPlayer();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchPause();
        }

        countdownTimer -= Time.deltaTime;
        timerText.text = ((int)Mathf.Ceil(countdownTimer)).ToString();

        if(countdownTimer < 0f)
        {
            countdownTimer = 0f;
            EndRound(-1);
        }
    }

    private void SwitchPause()
    {
        if (Time.timeScale == 0f)
        {
            announceText.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
            announceText.gameObject.SetActive(true);
            announceText.text = "PAUSE";
        }
    }

    private void EndRound(int winnerid)
    {
        bool istimeup = false;
        if (winnerid == -1)
        {
            winnerid = playerInfos[0].CurrentHealthPoint > playerInfos[1].CurrentHealthPoint ? 0 : 1;
            istimeup = true;
        }

        playerWinCount[winnerid]++;
        roundCounter++;
        Debug.LogFormat("Player {0} win {1} round", winnerid, playerWinCount[winnerid]);
        StartCoroutine(EndRoundDisplay(istimeup, winnerid));
    }

    private void EndBattle()
    {
        StartCoroutine("EndBattleDisplay");
    }

    private void CharacterDie(int id)
    {
        id = id * -1 + 1;
        EndRound(id);
    }

    IEnumerator StartRoundDisplay(int roundNumber)
    {
        GameManager.Instance.PauseTime(true);
        announceText.gameObject.SetActive(true);
        announceText.text = string.Format("Round {0}", roundNumber);
        yield return new WaitForSecondsRealtime(3);
        announceText.text = "Fight!";
        yield return new WaitForSecondsRealtime(1);
        announceText.gameObject.SetActive(false);
        GameManager.Instance.PauseTime(false);
    }

    IEnumerator EndRoundDisplay(bool isTimeUp, int winnerId)
    {
        Time.timeScale = 0;
        announceText.gameObject.SetActive(true);
        announceText.text = isTimeUp ? "Time's Up!" : "Knock Out!";
        informationSets[winnerId].LightBubble(playerWinCount[winnerId]);
        yield return new WaitForSecondsRealtime(3);
        announceText.gameObject.SetActive(false);
        Time.timeScale = 1;

        if (playerWinCount[winnerId] == maxWinRound)
        {
            StartCoroutine(EndBattleDisplay(winnerId));
        }
        else
        {
            StartNewRound();
        }
    }

    IEnumerator EndBattleDisplay(int winnerId)
    {
        Time.timeScale = 0;
        announceText.gameObject.SetActive(true);
        announceText.text = string.Format("Player {0} Win!", winnerId + 1);
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;

        SceneManager.LoadScene("Menu");
    }
}
