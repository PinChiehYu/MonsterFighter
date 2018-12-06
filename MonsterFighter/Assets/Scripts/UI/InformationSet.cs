using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationSet : MonoBehaviour {

    [SerializeField]
    private int playerId;

    private Image HPBar;
    private Image MPBar;
    private Image[] Lights = new Image[2];

    //private Image EPBar;
    //private BattleManager battleManager;

    private float newHpPercentage, newMpPercentage;

    void Awake ()
    {
        HPBar = transform.Find("HpBar").GetComponent<Image>();
        MPBar = transform.Find("MpBar").GetComponent<Image>();
        Lights[0] = transform.Find("Bubble_0").GetComponent<Image>();
        Lights[1] = transform.Find("Bubble_1").GetComponent<Image>();
        newHpPercentage = 1f;
        newMpPercentage = 1f;
    }
	
	void Update () {
        HPBar.fillAmount = Mathf.Lerp(HPBar.fillAmount, newHpPercentage, 0.1f);
        MPBar.fillAmount = Mathf.Lerp(MPBar.fillAmount, newMpPercentage, 0.1f);
    }

    public void OnPlayerHpChange(float percentage)
    {
        newHpPercentage = percentage;
    }

    public void OnPlayerMpChange(float percentage)
    {
        newMpPercentage = percentage;
    }

    public void LightBubble(int lightId)
    {
        StartCoroutine("DisplayLight", lightId - 1);
    }

    IEnumerator DisplayLight(int id)
    {
        while(Lights[id].fillAmount < 1f)
        {
            Lights[id].fillAmount = Mathf.Lerp(Lights[id].fillAmount, 1f, 0.05f);
            yield return null;
        }
    }
}
