using UnityEngine;
using UnityEngine.Audio;

public class PointsOnCollide : MonoBehaviour
{
    public int pointValue;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioMixerGroup audioMixerGroup;
    public AudioClip collectSound;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        GameManager.instance.score += pointValue;
        PlayCollectSound();
        Destroy(gameObject);
    }
    
    private void PlayCollectSound()
    {
        if (audioSource && collectSound)
        {
            GameObject soundObject = new GameObject("Death SFX");
            soundObject.transform.position = transform.position;

            AudioSource source = soundObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.clip = collectSound;
            if (audioMixerGroup)
            {
                source.outputAudioMixerGroup = audioMixerGroup;
            }
            source.spatialBlend = 1f;

            source.Play();

            Destroy(soundObject, collectSound.length);
        }
    }
}