using UnityEngine;
using System.Collections.Generic;

public class ListActiveAudioSources : MonoBehaviour
{
    void Start()
    {
        AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        List<AudioSource> activeAudioSources = new List<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                activeAudioSources.Add(audioSource);
            }
        }

        Debug.Log("Active Audio Sources:");
        foreach (AudioSource audioSource in activeAudioSources)
        {
            Debug.Log(audioSource.name);
        }
    }
}
