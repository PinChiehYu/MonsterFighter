using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {

    private Dictionary<string, KeyCode>[] PlayerControlSets = new Dictionary<string, KeyCode>[2];

    private Button currentButton;
    private string buttonTextTemp;
    private string[] ControlType = new string[] { "Up", "Down", "Right", "Left", "Attack1", "Attack2" };
    private string[] defaultControlKeyCode = new string[] { "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Comma", "Period" };

    void Awake ()
    {
        for(int i = 0; i < 2; i++)
        {
            PlayerControlSets[i] = new Dictionary<string, KeyCode>();
            for (int j = 0; j < ControlType.Length; j++)
            {
                string keycode = PlayerPrefs.GetString(i.ToString() + "_" + ControlType[j], defaultControlKeyCode[j]);
                PlayerControlSets[i].Add(ControlType[j], (KeyCode)Enum.Parse(typeof(KeyCode), keycode));
            }
        }
        GameManager.Instance.playerControlSets = PlayerControlSets;
    }

    void OnGUI()
    {
        if(currentButton != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                int playerNumber = Int32.Parse(currentButton.name.Split(new char[] { '_' })[0]);
                string controlType = currentButton.name.Split(new char[] { '_' })[1];
                if (e.keyCode != KeyCode.None && (PlayerControlSets[playerNumber][controlType] == e.keyCode || !PlayerControlSets[playerNumber].ContainsValue(e.keyCode)))
                {
                    //Debug.Log(playerNumber.ToString() + ":" + controlType + ":" + e.keyCode.ToString());
                    PlayerControlSets[playerNumber][controlType] = e.keyCode;
                    GetButtonText(currentButton).text = e.keyCode.ToString();
                    GetButtonText(currentButton).fontStyle = FontStyles.Bold;
                    currentButton = null;
                }
            }
        }
    }

    public void StartAssignment(Button button)
    {
        if(currentButton != null)
        {
            GetButtonText(currentButton).text = buttonTextTemp;
            GetButtonText(currentButton).fontStyle = FontStyles.Bold;
        }

        currentButton = button;
        buttonTextTemp = GetButtonText(button).text;
        GetButtonText(button).text = "Press a Key";
        GetButtonText(button).fontStyle = FontStyles.Italic;
    }

    public void SaveSetting()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < ControlType.Length; j++)
            {
                PlayerPrefs.SetString(i.ToString() + "_" + ControlType[j], PlayerControlSets[i][ControlType[j]].ToString());
            }
        }
        PlayerPrefs.Save();
        GameManager.Instance.playerControlSets = PlayerControlSets;
        GameObject.Find("ControlSettingPanel").SetActive(false);
    }

    public void UpdateButtonText()
    {
        List<Button> button_list = new List<Button>();
        GameObject.Find("ControlSettingPanel").GetComponentsInChildren(button_list);

        foreach(Button B in button_list)
        {
            if (B.name == "Save") break;

            int playerNumber = Int32.Parse(B.name.Split(new char[] { '_' })[0]);
            string controlType = B.name.Split(new char[] { '_' })[1];
            GetButtonText(B).text = PlayerControlSets[playerNumber][controlType].ToString();
        }
    }

    private TMP_Text GetButtonText(Button button)
    {
        return button.GetComponentInChildren<TMP_Text>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
