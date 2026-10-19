using UnityEngine;
using UnityEngine.Audio;

public class Bullet : MonoBehaviour
{
    public float fireForce = 25f;
    public float lifetime = 10f;
    
    private Rigidbody rigidBody;
    
    public AudioClip shootSfx;
    
    public AudioMixerGroup shootMixerGroup;

    private void Start()
    {
        
        PlayShootSound();
        
        rigidBody = GetComponent<Rigidbody>();
        Debug.DrawRay(
            transform.position,
            transform.up * 5f,
            Color.green,
            5f
        );
        if (rigidBody)
        {
            rigidBody.AddForce(transform.up * fireForce, ForceMode.Impulse);
        }
        
        Destroy(gameObject, lifetime);
    }
    
    // Similar to how "PlayClipAtPoint" works
    private void PlayShootSound()
    {
        if (shootSfx)
        {
            GameObject soundObject = new GameObject("Bullet Shoot SFX");
            soundObject.transform.position = transform.position;

            AudioSource source = soundObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.clip = shootSfx;
            if (shootMixerGroup)
            {
                source.outputAudioMixerGroup = shootMixerGroup;
            }
            source.spatialBlend = 1f;

            source.Play();

            Destroy(soundObject, shootSfx.length);
        }
    }
}
