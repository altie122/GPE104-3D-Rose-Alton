using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float currentHealth = 100;
    public float maxHealth = 100;
    public bool useDeathComponent = true;
    public Image healthBar;

    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioMixerGroup audioMixerGroup;
    public AudioClip takeDamageSound;
    public AudioClip deathSound;

    private DeathManager deathComponent;
    private void Start()
    {
        if (useDeathComponent)
        {
            deathComponent = GetComponent<DeathManager>();
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }
    
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (audioSource && takeDamageSound)
        {
            if (audioMixerGroup)
            {
                audioSource.outputAudioMixerGroup = audioMixerGroup;
            }
            
            audioSource.PlayOneShot(takeDamageSound);
        }
        
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0 && deathComponent != null)
        {
            PlayDeathSound();
            deathComponent.Die();
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource && deathSound)
        {
            GameObject soundObject = new GameObject("Death SFX");
            soundObject.transform.position = transform.position;

            AudioSource source = soundObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.clip = deathSound;
            if (audioMixerGroup)
            {
                source.outputAudioMixerGroup = audioMixerGroup;
            }
            source.spatialBlend = 1f;

            source.Play();

            Destroy(soundObject, deathSound.length);
        }
    }
}