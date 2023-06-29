using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class settingsManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] private Image barraFX;
    
    [SerializeField] private Image barraSom;

    [SerializeField] private Button mainMenuButton;

    private int indexSceneMainMenu = 0;

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

    void Start()
    {  
        if (SceneManager.GetActiveScene().buildIndex == indexSceneMainMenu) // se a cena atual for o menu principal
        {
            mainMenuButton.gameObject.SetActive(false);
        }
        else 
        {
            mainMenuButton.gameObject.SetActive(true);
        }
    }

    public void openSettings()
    {
        canvas.GetComponent<Canvas>().enabled = true;
    }

    public void quit()
    {
        canvas.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }

    public void goToMainMenu()
    {
        canvas.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
