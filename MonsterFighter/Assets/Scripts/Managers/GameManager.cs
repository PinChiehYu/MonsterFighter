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



    public GameObject CreateCharacter(int playerId)
    {
        return characterFactory.CreateCharacter(playerId, playerCharacterPicks[playerId], playerControlSets[playerId]);
    }

}
