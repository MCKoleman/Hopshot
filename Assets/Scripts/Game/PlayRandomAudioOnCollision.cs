using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomAudioOnCollision : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (clips.Length == 0 || audioSource.isPlaying)
            return;
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
