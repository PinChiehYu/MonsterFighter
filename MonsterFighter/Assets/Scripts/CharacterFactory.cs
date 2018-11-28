using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterFactory
{
    private List<string> characterNameList = new List<string>();
    private Dictionary<string, GameObject> characterDictionary = new Dictionary<string, GameObject>();
    private Vector3[] StartPosition = new Vector3[2];

    public CharacterFactory()
    {
        characterNameList.Add("UnityChan");
        characterNameList.Add("CatFighter");
        characterNameList.Add("Coco");
        characterNameList.Add("Rock");
        characterNameList.Add("Test");
        LoadAllCharacterPrefabs();

        StartPosition[0] = new Vector3(-4f, -3f);
        StartPosition[1] = new Vector3(4f, -3f);
    }

    private void LoadAllCharacterPrefabs()
    {
        foreach (string name in characterNameList)
        {
            characterDictionary.Add(name, Resources.Load<GameObject>("Prefabs/Characters/" + name));
            if (characterDictionary[name] != null)
            {
                Debug.Log("Load Prefab Success:" + name);
            }
            else
            {
                Debug.LogWarning("Load Prefab Fail:" + name);
            }
        }
    }

    public GameObject CreateCharacter(int playerId, string characterName, Dictionary<string, KeyCode> controlSet)
    {
        GameObject character = UnityEngine.Object.Instantiate(characterDictionary[characterName]);
        character.GetComponent<PlayerInfo>().id = playerId;
        character.GetComponent<PlayerController>().SetupController(controlSet, StartPosition[playerId]);

        return character;
    }
}
