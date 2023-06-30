using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public Image barraSom;

    [SerializeField] public Image barraFX;

    [SerializeField] public AudioSource _audioSource, _effectsSource;

    [SerializeField] private AudioSource upgradeSound, sellSound, nexusDMG, attack1, attack2, attack3;

    public bool _isEffectsOn = true;

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
        if (_isEffectsOn)
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

    public void PlayAttackSound()
    {
        if (_isEffectsOn)
        {
            int number = Random.Range(1, 3);
            switch (number)
            {
                case 1: attack1.Play(); break;
                case 2: attack2.Play(); break;
                case 3: attack3.Play(); break;
                default: break;
            }
        }
    }

    public void PlayNexusDMGSound()
    {
        if (_isEffectsOn)
        {
            nexusDMG.Play();
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
