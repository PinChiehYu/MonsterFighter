using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager
{
    private static object m_oLock = new object();
    private static GameManager m_oInstance = null;

    public Dictionary<string, KeyCode>[] playerControlSets;
    public string[] playerCharacterPicks;
    public string gameMode;

    private float fixedtime;

    private CharacterFactory characterFactory;

    public static GameManager Instance
    {
        get
        {
            lock (m_oLock)
            {
                if (m_oInstance == null)
                {
                    Debug.Log("GameManager Instanciated");
                    m_oInstance = new GameManager();
                }
                return m_oInstance;
            }
        }
    }

    public GameManager()
    {
        playerControlSets = new Dictionary<string, KeyCode>[2];
        playerCharacterPicks = new string[2];

        characterFactory = new CharacterFactory();
    }

    public GameObject CreateCharacter(int playerId, Vector3 startPosition, Vector3 falloutPosition)
    {
        return characterFactory.CreateCharacter(playerId, playerCharacterPicks[playerId], playerControlSets[playerId], startPosition, falloutPosition);
    }

    public void PauseTime(bool yes)
    {
        if (yes)
        {
            fixedtime = Time.fixedDeltaTime;
            Time.timeScale = 0;
        }
        else
        {
            Time.fixedDeltaTime = fixedtime;
            Time.timeScale = 1;
        }
    }

    public void SwitchScene(string sceneName)
    {

    }
}
