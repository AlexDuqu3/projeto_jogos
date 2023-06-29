using System;

[Serializable]
public class HighScoreElement
{
    public string date;
    public int score;


    public HighScoreElement(string date, int score)
    {
        this.date = date;
        this.score = score;

    }
}
