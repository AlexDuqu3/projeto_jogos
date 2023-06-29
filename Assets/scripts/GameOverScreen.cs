using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text scoreText;
    public void Setup(int score, int waveNumber) {

        gameObject.SetActive(true);
        HighScoreHandler.Instance.AddScore(score);
        scoreText.text = "<color=yellow>Points</color>: " + score.ToString() + " | " + "<color=white>Wave</color>: " + waveNumber.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
