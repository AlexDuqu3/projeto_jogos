using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] private Image barraFX;

    [SerializeField] private Image barraSom;

    private void Awake()
    {
        

        if (SoundManager.Instance && SoundManager.Instance._audioSource.mute == true)
        {
            barraSom.enabled = true;
        }
        else
        {
            barraSom.enabled = false;
        }

        if (SoundManager.Instance && SoundManager.Instance._isEffectsOn == false)
        {
            barraFX.enabled = true;
        }
        else
        {
            barraFX.enabled = false;
        }

        if (SoundManager.Instance) {
            SoundManager.Instance.barraSom = barraSom;
            SoundManager.Instance.barraFX = barraFX;
        }
  
    }

    public void openSettings() {
        canvas.GetComponent<Canvas>().enabled = true;

    }

    public void quit()
    {
        canvas.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }
}
