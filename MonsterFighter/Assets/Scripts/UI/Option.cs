using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : Singleton<Option>
{
    private GameObject UIGroup;
    bool turnOn;

    void Start()
    {
        UIGroup = transform.Find("UIGroup").gameObject;
        UIGroup.SetActive(false);
        turnOn = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            SwitchState();
        }
	}

    private void SwitchState()
    {
        turnOn = !turnOn;
        UIGroup.SetActive(turnOn);
        Time.timeScale = turnOn ? 0f : 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneSwitcher.instance.SwitchScene("Menu");
        SwitchState();
    }
}
