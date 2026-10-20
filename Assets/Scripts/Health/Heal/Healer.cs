using System;
using UnityEngine;
using UnityEngine.Audio;

public class Healer : MonoBehaviour
{
    public float healAmount;

    public bool isInstaHeal;
    
    private Health otherHealthComponent;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioMixerGroup audioMixerGroup;
    public AudioClip healSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        otherHealthComponent = collider.GetComponent<Health>();
        
        // if instaHeal, make the other object gain max health, else make it gain health equal to healAmount
        otherHealthComponent.Heal(isInstaHeal ? otherHealthComponent.maxHealth : healAmount);
        PlayHealSound();
        Destroy(gameObject);
    }
    
    private void PlayHealSound()
    {
        if (audioSource && healSound)
        {
            GameObject soundObject = new GameObject("Death SFX");
            soundObject.transform.position = transform.position;

            AudioSource source = soundObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.clip = healSound;
            if (audioMixerGroup)
            {
                source.outputAudioMixerGroup = audioMixerGroup;
            }
            source.spatialBlend = 1f;

            source.Play();

            Destroy(soundObject, healSound.length);
        }
    }
}