using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour {
    // Clips to loop between
    public AudioClip[] clips;
    public AudioSource audioSource;
    void Start () {
        audioSource.loop = false;
    }
    // Change to random song
    public AudioClip GetClip()
    {
        return clips [Random.Range (0, clips.Length)];
    }

    void Update () {
        // If not playing, change to random song
        if (!audioSource.isPlaying)
        {
            Debug.Log("Picking");
            audioSource.clip = GetClip();
            audioSource.Play();
        }
    }
}