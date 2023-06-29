using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    public static HighScoreHandler Instance;

    public List<HighScoreElement> highScorelist = new List<HighScoreElement>();
    [SerializeField] int maxCount = 3;
    [SerializeField] string filename;

    private void Awake()
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

    private void Start()
    {
        LoadHighScores();
        Debug.Log("highScorelist.Count");

        Debug.Log(highScorelist.Count);

    }

    private void LoadHighScores()
    {
        highScorelist = FileHandler.ReadListFromJSON<HighScoreElement>(filename);
        while (highScorelist.Count > maxCount)
        {
            highScorelist.RemoveAt(maxCount);
        }
    }
    public void SaveHighScore()
    {
        FileHandler.SaveToJSON<HighScoreElement> (highScorelist, filename);
    }



    public void AddScore(int score)
    {
        DateTime now = DateTime.Now;
        HighScoreElement element = new HighScoreElement($"{now.Day}/{now.Month} {now.Hour}:{now.Minute}h",score);
        AddHighscoreIfPossible(element);
    }

    private void AddHighscoreIfPossible(HighScoreElement element)
    {
        for (int i = 0;i< maxCount;i++)
        {
            if (i >= highScorelist.Count || element.score > highScorelist[i].score)
            {
                highScorelist.Insert(i, element);
                Debug.Log("adicionou score");

                while (highScorelist.Count > maxCount)
                {
                    highScorelist.RemoveAt(maxCount);
                }

                break;
            }

        }
    }
}
