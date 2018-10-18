using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterFactory
{
    private List<string> CharacterNameList = new List<string>();
    private Dictionary<string, GameObject> CharacterDictionary = new Dictionary<string, GameObject>();
    private Vector3[] StartPosition = new Vector3[2];

    public CharacterFactory()
    {
        CharacterNameList.Add("UnityChan");
        CharacterNameList.Add("CatFighter");
        CharacterNameList.Add("Toko");
        LoadAllCharacterPrefabs();

        StartPosition[0] = new Vector3(-1.2f, 0);
        StartPosition[1] = new Vector3(1.2f, 0);
    }

    private void LoadAllCharacterPrefabs()
    {
        foreach (string name in CharacterNameList)
        {
            CharacterDictionary.Add(name, Resources.Load<GameObject>("Prefabs/Characters/" + name));
            if (CharacterDictionary[name] != null) Debug.Log("Load Prefab Success:" + name);
        }
    }

    public GameObject CreateCharacter(int playerId, string characterName, Dictionary<string, KeyCode> controlSet)
    {
        GameObject character = UnityEngine.Object.Instantiate(CharacterDictionary[characterName], StartPosition[playerId], Quaternion.identity);
        character.GetComponent<PlayerController>().SetControlSet(controlSet);
        character.GetComponent<PlayerController>().SetPlayerId(playerId);

        return character;
    }
}
