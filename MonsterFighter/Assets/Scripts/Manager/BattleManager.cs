using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class BattleManager : MonoBehaviour {

    [SerializeField]
    private int maxWinRound;
    [SerializeField]
    private int maxTimePerRound;

    private int roundCounter;
    private float countdownTimer;

    private Information information;

    private int[] playerComboHit = new int[2] { 0, 0 };

    private GameObject[] playerChars = new GameObject[2];
    private int[] playerWinCount = new int[2] { 0, 0 };

    private CinemachineTargetGroup targetGroup;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private List<Vector3> pivotList;

    void Awake ()
    {
        roundCounter = 1;

        information = GameObject.Find("Information").GetComponent<Information>();
        //comboSets = GameObject.Find("Information").GetComponentsInChildren<Combo>();
        pivotList = GameObject.Find("PivotSet").GetComponent<PivotSet>().GetPivotsPosition();
        targetGroup = GameObject.Find("CharGroup").GetComponent<CinemachineTargetGroup>();
        cameraNoise = GameObject.Find("CMvcam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        InstantiateCharacters();
        RegisterEvent();
        StartNewRound();
    }

    private void InstantiateCharacters()
    {
        playerChars[0] = GameManager.Instance.CreateCharacter(0, pivotList[0], pivotList[1]);
        playerChars[1] = GameManager.Instance.CreateCharacter(1, pivotList[2], pivotList[3]);

        targetGroup.m_Targets[0].target = playerChars[0].transform;
        targetGroup.m_Targets[0].radius = 3;
        targetGroup.m_Targets[1].target = playerChars[1].transform;
        targetGroup.m_Targets[1].radius = 3;
        //cameraNoise.
    }

    private void RegisterEvent()
    {
        for (int i = 0; i < 2; i++)
        {
            PlayerInfo plyinf = playerChars[i].GetComponent<PlayerInfo>();
            plyinf.OnDie += CharacterDie;
            plyinf.OnHpChange += information.OnPlayerHpChange(i);
            plyinf.OnMpChange += information.OnPlayerMpChange(i);
            //plyinf.OnHpChange += comboSets[i].OnPlayerHpChange;
            playerChars[i].GetComponent<CombatHandler>().OnReceiveCrit += StartShaking;
        }
    }

    private void StartNewRound()
    {
        countdownTimer = maxTimePerRound;
        CleanUpProjectiles();
        ResetPlayers();
        StartCoroutine(StartRoundDisplay(roundCounter));
    }

    private void CleanUpProjectiles()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject obj in list)
        {
            Destroy(obj);
        }
    }

    private void ResetPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            playerChars[i].GetComponent<PlayerInfo>().ResetPlayerInfo();
            playerChars[i].GetComponent<PlayerController>().ResetController();
            playerChars[i].GetComponent<PhysicsObject>().ResetPhysics();

            targetGroup.m_Targets[i].weight = 1;
        }
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchPause();
        }

        countdownTimer -= Time.deltaTime;
        information.UpdateTimer((int)Mathf.Ceil(countdownTimer));

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
            information.TurnOffAnnounce();
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
            information.TurnOnAnnounce("PAUSE");
        }
    }

    private void EndRound(int winnerId)
    {
        bool istimeup = false;
        if (winnerId == -1)
        {
            winnerId = playerChars[0].GetComponent<PlayerInfo>().CurrentHealthPoint > playerChars[1].GetComponent<PlayerInfo>().CurrentHealthPoint ? 0 : 1;
            istimeup = true;
        }

        playerWinCount[winnerId]++;
        roundCounter++;
        Debug.LogFormat("Player {0} win {1} round", winnerId, playerWinCount[winnerId]);
        StartCoroutine(EndRoundDisplay(istimeup, winnerId));
    }

    private void EndBattle()
    {
        playerChars[0].GetComponent<PlayerController>().SetInputActivate(false, false);
        playerChars[1].GetComponent<PlayerController>().SetInputActivate(false, false);
        StartCoroutine("EndBattleDisplay");
    }

    private void CharacterDie(int id)
    {
        targetGroup.m_Targets[id].weight = 0;
        EndRound(id * -1 + 1);
    }

    IEnumerator StartRoundDisplay(int roundNumber)
    {
        Time.timeScale = 0;
        information.TurnOnAnnounce(string.Format("Round {0}", roundNumber));
        yield return new WaitForSecondsRealtime(3);
        information.TurnOnAnnounce("Fight!");
        yield return new WaitForSecondsRealtime(1);
        information.TurnOffAnnounce();
        WakeUpPlayers();
        Time.timeScale = 1;
    }

    private void WakeUpPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            playerChars[i].GetComponent<PlayerController>().WakeUp();
        }
    }

    IEnumerator EndRoundDisplay(bool isTimeUp, int winnerId)
    {
        Time.timeScale = 0;
        information.TurnOnAnnounce(isTimeUp ? "Time's Up!" : "Knock Out!");
        information.LightUpBubble(winnerId, playerWinCount[winnerId]);
        yield return new WaitForSecondsRealtime(3);
        information.TurnOffAnnounce();
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
        information.TurnOnAnnounce(string.Format("Player {0} Win!", winnerId + 1));
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;

        SceneManager.LoadScene("Menu");
    }

    private void StartShaking()
    {
        if (shake != null) StopCoroutine(shake);
        shake = ShakeCamera(0.1f);
        StartCoroutine(shake);
    }

    private IEnumerator shake;
    private IEnumerator ShakeCamera(float duration)
    {
        cameraNoise.m_AmplitudeGain = 1f;
        cameraNoise.m_FrequencyGain = 100f;
        yield return new WaitForSecondsRealtime(duration);
        cameraNoise.m_AmplitudeGain = 0f;
        cameraNoise.m_FrequencyGain = 0f;
    }
}
