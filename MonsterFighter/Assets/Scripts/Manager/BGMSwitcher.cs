using UnityEngine;
using System.Collections;

public class BGMSwitcher : MonoBehaviour
{

    public AudioClip selectionMusic;


    private AudioSource source;


    // Use this for initialization
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }


    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            source.clip = selectionMusic;
            source.Play();
        }

    }
}