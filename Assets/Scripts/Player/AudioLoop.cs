using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour {
    public AudioClip[] clips;
    public AudioSource audioSource;
    void Start () {
        audioSource.loop = false;
    }

    public AudioClip GetClip()
    {
        return clips [Random.Range (0, clips.Length)];
    }

    void Update () {
        if (!audioSource.isPlaying)
        {
            Debug.Log("Picking");
            audioSource.clip = GetClip();
            audioSource.Play();
        }
    }
}