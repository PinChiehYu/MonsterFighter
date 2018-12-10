using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterFactory
{
    private List<string> characterNameList = new List<string>();
    private Dictionary<string, GameObject> characterDictionary = new Dictionary<string, GameObject>();
    private Vector3[] StartPosition = new Vector3[2];
    private Vector3[] FalloutPosition = new Vector3[2];

    public CharacterFactory()
    {
        characterNameList.Add("Coco");
        characterNameList.Add("Rock");
        characterNameList.Add("DK");
        characterNameList.Add("UnityChan");
        LoadAllCharacterPrefabs();

        StartPosition[0] = new Vector3(-4f, -3f, 0f);
        StartPosition[1] = new Vector3(4f, -3f, 0f);
        FalloutPosition[0] = new Vector3(5f, 0f, 0f);
        FalloutPosition[1] = new Vector3(5f, 0f, 0f);
    }

    private void LoadAllCharacterPrefabs()
    {
        foreach (string name in characterNameList)
        {
            characterDictionary.Add(name, Resources.Load<GameObject>("Prefabs/Characters/" + name));
            if (characterDictionary[name] == null)
            {
                Debug.LogWarning("Load Prefab Fail:" + name);
            }
        }
    }

    public GameObject CreateCharacter(int playerId, string characterName, Dictionary<string, KeyCode> controlSet, Vector3 startPoint, Vector3 falloutPoint)
    {
        GameObject character = Object.Instantiate(characterDictionary[characterName]);
        character.name = playerId.ToString();
        character.GetComponent<PlayerController>().SetupController(controlSet, startPoint, falloutPoint);

        return character;
    }
}
