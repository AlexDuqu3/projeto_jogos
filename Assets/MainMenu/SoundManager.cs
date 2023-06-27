using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private Image barraSom;

    [SerializeField] private Image barraFX;

    [SerializeField] private AudioSource _audioSource, _effectsSource;

    [SerializeField] private AudioSource upgradeSound, sellSound;

    private bool _isEffectsOn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioSource clip)
    {
        Debug.Log(clip.name);
        if (_isEffectsOn)
        {
            clip.Play();
        }
    }

    public void PlayAudioSource(AudioSource clip)
    { 
        if (_isEffectsOn) {
            clip.Play();
        }
        
    }

    public void PlaySellSound()
    {
        if (true)
        {
            sellSound.Play();
        }
        
    }

    public void PlayUpgradeSound()
    {
        if (_isEffectsOn)
        {
            upgradeSound.Play();
        } 
    }



    public void ToggleMusic()
    {
        _audioSource.mute = !_audioSource.mute;

        if (_audioSource.mute == true) {
            barraSom.enabled = true;
        }
        else
        {
            barraSom.enabled = false;
        }
    }

    public void ToggleFX()
    {
        _isEffectsOn = !_isEffectsOn;

        Debug.Log(_isEffectsOn.ToString());
        if (_isEffectsOn == false)
        {
            barraFX.enabled = true;
        }
        else
        {
            barraFX.enabled = false;
        }
    }

    public void ChangeMasterVolume(float value) {
        AudioListener.volume = value;
    }

}
