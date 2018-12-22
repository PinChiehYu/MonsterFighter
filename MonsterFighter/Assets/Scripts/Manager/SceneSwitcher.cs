using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitcher : Singleton<SceneSwitcher>
{
    public List<AudioClip> musicList;
    private AudioSource audioSource;
    private Image masking;

    private string currentScene;

    public event Action OnSecenLoaded;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        masking = GetComponentInChildren<Image>();
    }

    void Start()
    {
        SwitchSong();
    }

    private void SwitchSong()
    {
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        if (sceneId < musicList.Count)
        {
            audioSource.Stop();
            audioSource.clip = musicList[sceneId];
            audioSource.Play();
        }
    }

    public void SwitchScene(string sceneName)
    {
        OnSecenLoaded = null;
        currentScene = sceneName;
        StartCoroutine(StartSwitchScene(sceneName));
    }

    public void ReloadScene()
    {
        StartCoroutine(StartSwitchScene(currentScene));
    }

    IEnumerator StartSwitchScene(string sceneName)
    {
        AsyncOperation async;

        Color color = masking.color;
        while (color.a < 1)
        {
            color.a += 0.05f;
            masking.color = color;
            audioSource.volume = (1 - color.a) * 0.5f;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        async = SceneManager.LoadSceneAsync(sceneName);
        while (async.isDone)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        SwitchSong();
        color = masking.color;
        while (color.a > 0)
        {
            color.a -= 0.05f;
            masking.color = color;
            audioSource.volume = (1 - color.a) * 0.5f;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        OnSecenLoaded?.Invoke();
    }
}