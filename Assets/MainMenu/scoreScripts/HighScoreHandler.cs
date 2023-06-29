using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    public static HighScoreHandler Instance;

    public List<HighScoreElement> highScorelist = new List<HighScoreElement>();
    [SerializeField] int maxCount = 3;

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

    }

    private void LoadHighScores()
    {
        AddHighscoreIfPossible(new HighScoreElement("1", 1));
        AddHighscoreIfPossible(new HighScoreElement("2", 2));
        AddHighscoreIfPossible(new HighScoreElement("3", 3));
        AddHighscoreIfPossible(new HighScoreElement("4", 4));
        AddHighscoreIfPossible(new HighScoreElement("5", 5));


    }

    public void AddHighscoreIfPossible(HighScoreElement element)
    {
        for (int i = 0;i< maxCount;i++)
        {
            if (i >= highScorelist.Count || element.score > highScorelist[i].score)
            {
                highScorelist.Insert(i, element);

                while (highScorelist.Count > maxCount)
                {
                    highScorelist.RemoveAt(maxCount);
                }

                break;
            }

        }
    }
}
