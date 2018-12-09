using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BGMSwitcher : MonoBehaviour
{

    public List<AudioClip> musicList;

    private AudioSource audioSource;


    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneId = scene.buildIndex;
        Debug.Log(sceneId);
        if (sceneId < musicList.Count)
        {
            audioSource.Stop();
            audioSource.clip = musicList[sceneId];
            audioSource.Play();
        }
    }
}