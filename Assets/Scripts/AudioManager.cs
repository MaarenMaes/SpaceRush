using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("-------- Audio Source --------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("-------- Audio Clip --------")]
    public AudioClip _backgroundMusic;
    public AudioClip _shoot;
    public AudioClip _explosion;
    public AudioClip _pickup;
    public AudioClip _shieldHit;
    public AudioClip _lazers;
    public AudioClip _upgrade;
    public AudioClip _denied;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource.clip = _backgroundMusic;
            musicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
