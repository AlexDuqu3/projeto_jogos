using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Android;
    

public class MainMenu : MonoBehaviour
{
    private const string permission = Permission.ExternalStorageWrite;

    private void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuiGame() {

        HighScoreHandler.Instance.SaveHighScore();
        Application.Quit();
    }

}
