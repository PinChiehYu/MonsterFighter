using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {

    private Dictionary<string, KeyCode>[] playerControlSets = new Dictionary<string, KeyCode>[2];

    private Button currentButton;
    private string buttonTextTemp;
    private string[] controlType = new string[] { "Up", "Down", "Right", "Left", "AtkL", "AtkH", "SklS", "SklB" };
    private string[] defaultControlKeyCode = new string[] { "UpArrow", "DownArrow", "RightArrow", "LeftArrow", "Comma", "Period", "K", "H" };

    void Awake ()
    {
        for(int i = 0; i < 2; i++)
        {
            playerControlSets[i] = new Dictionary<string, KeyCode>();
            for (int j = 0; j < controlType.Length; j++)
            {
                string defaultKeyCode = PlayerPrefs.GetString(i.ToString() + "_" + controlType[j], defaultControlKeyCode[j]);
                playerControlSets[i].Add(controlType[j], (KeyCode)Enum.Parse(typeof(KeyCode), defaultKeyCode));
            }
        }
        GameManager.Instance.playerControlSets = playerControlSets;
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
                if (e.keyCode != KeyCode.None && (playerControlSets[playerNumber][controlType] == e.keyCode || !playerControlSets[playerNumber].ContainsValue(e.keyCode)))
                {
                    playerControlSets[playerNumber][controlType] = e.keyCode;
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
            for (int j = 0; j < controlType.Length; j++)
            {
                PlayerPrefs.SetString(i.ToString() + "_" + controlType[j], playerControlSets[i][controlType[j]].ToString());
            }
        }
        PlayerPrefs.Save();
        GameManager.Instance.playerControlSets = playerControlSets;
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
            GetButtonText(B).text = playerControlSets[playerNumber][controlType].ToString();
        }
    }

    private TMP_Text GetButtonText(Button button)
    {
        return button.GetComponentInChildren<TMP_Text>();
    }

    public void StartGame(string mode)
    {
        GameManager.Instance.gameMode = mode;
        SceneManager.LoadScene("Select");
    }
}
