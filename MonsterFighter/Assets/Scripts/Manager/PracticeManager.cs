using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PracticeManager : MonoBehaviour {

    private Information information;

    private int[] playerComboHit = new int[2] { 0, 0 };

    private GameObject[] playerChars = new GameObject[2];
    private int[] playerWinCount = new int[2] { 0, 0 };

    private CinemachineTargetGroup targetGroup;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private List<Vector3> pivotList;

    // Use this for initialization
    void Awake()
    {
        information = GameObject.Find("Information").GetComponent<Information>();
        //comboSets = GameObject.Find("Information").GetComponentsInChildren<Combo>();
        pivotList = GameObject.Find("PivotSet").GetComponent<PivotSet>().GetPivotsPosition();
        targetGroup = GameObject.Find("CharGroup").GetComponent<CinemachineTargetGroup>();
        cameraNoise = GameObject.Find("CMvcam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start () {
        InstantiateCharacters();
        RegisterEvent();
        StartNewRound();
    }

    private void InstantiateCharacters()
    {
        playerChars[0] = GameManager.Instance.CreateCharacter(0, pivotList[0], pivotList[1]);
        playerChars[1] = GameManager.Instance.CreateDummy(pivotList[2], pivotList[3]);

        targetGroup.m_Targets[0].target = playerChars[0].transform;
        targetGroup.m_Targets[0].radius = 3;
        targetGroup.m_Targets[1].target = playerChars[1].transform;
        targetGroup.m_Targets[1].radius = 3;
    }

    private void RegisterEvent()
    {
        for (int i = 0; i < 2; i++)
        {
            PlayerInfo plyinf = playerChars[i].GetComponent<PlayerInfo>();
            plyinf.OnHpChange += information.OnPlayerHpChange(i);
            plyinf.OnMpChange += information.OnPlayerMpChange(i);
            plyinf.OnKnockdownChange += information.OnPlayerKnockdownChange(i);
            //plyinf.OnHpChange += comboSets[i].OnPlayerHpChange;
            playerChars[i].GetComponent<CombatHandler>().OnReceiveCrit += StartShaking;
            playerChars[i].GetComponent<CombatHandler>().OnReceiveAttack += RestLockHpMp;
        }
    }

    private void StartNewRound()
    {
        CleanUpProjectiles();
        ResetPlayers();
        StartCoroutine(StartRoundDisplay());
    }

    private void CleanUpProjectiles()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject obj in list)
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchPause();
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

    IEnumerator StartRoundDisplay()
    {
        Time.timeScale = 0;
        information.TurnOnAnnounce(string.Format("-Practice Mode-"));
        yield return new WaitForSecondsRealtime(3);
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

    private void RestLockHpMp()
    {
        if (lockHpMp != null) StopCoroutine(lockHpMp);
        lockHpMp = LockHpMP();
        StartCoroutine(lockHpMp);
    }

    private IEnumerator lockHpMp;
    private IEnumerator LockHpMP()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                playerChars[i].GetComponent<PlayerInfo>().CurrentHealthPoint += 100f;
                playerChars[i].GetComponent<PlayerInfo>().CurrentManaPoint += 100f;
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
}
