using UnityEngine;
using System.Collections.Generic;
using System;

public enum SoundEffect
{
    Footstep,
    Collision,
    Hurt,
    EatPellet,
    EatEnemy,
    EatPowerUp
}

[Serializable]
public struct SoundEffectEntry
{
    public SoundEffect soundEffect;
    public AudioClip clip;
}

public class SoundEffectsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<SoundEffectEntry> soundEffects;

    void PlaySoundEffect(SoundEffectEntry soundEffectEntry)
    {
        SoundEffectEntry entry = soundEffects.Find(e => e.soundEffect == soundEffectEntry.soundEffect);
        if (entry.clip != null)
        {
            audioSource.PlayOneShot(entry.clip);
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + soundEffectEntry.soundEffect.ToString());
        }
    }

    void Start()
    {
        //PlaySoundEffect(soundEffects[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
