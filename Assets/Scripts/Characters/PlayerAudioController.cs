using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> jumpSounds;
    [SerializeField]
    private float jumpVolume = 0.7f;
    [SerializeField]
    private List<AudioClip> deathSounds;
    [SerializeField]
    private float deathVolume = 1.0f;
    [SerializeField]
    private List<AudioClip> footstepSounds;
    [SerializeField]
    private float footstepVolume = 0.8f;
    [SerializeField]
    private List<AudioClip> groundSounds;
    [SerializeField]
    private float groundVolume = 1.0f;

    private AudioSource source;

    private void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    public void Die() { PlayRandomClip(ref deathSounds, deathVolume); }
    public void Jump() { PlayRandomClip(ref jumpSounds, jumpVolume); }
    public void Footstep() { PlayRandomClip(ref footstepSounds, footstepVolume); }
    public void Land() { PlayRandomClip(ref groundSounds, groundVolume); }

    // Returns a random audio clip from the given list
    private void PlayRandomClip(ref List<AudioClip> clips, float volume)
    {
        if (clips == null || clips.Count == 0)
            return;

        source.Stop();
        source.volume = volume;
        source.clip = clips[Random.Range(0, clips.Count)];
        source.Play();
    }
}
