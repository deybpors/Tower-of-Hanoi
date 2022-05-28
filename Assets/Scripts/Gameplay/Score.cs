using System;
using System.Collections.Generic;

[Serializable]
public class Score
{
    public string playerName;
    public float time;
    public int moves;
    public int ringCount;

    public Score(string playerName, float time, int moves, int ringCount)
    {
        this.playerName = playerName;
        this.time = time;
        this.moves = moves;
        this.ringCount = ringCount;
    }
}

[Serializable]
public class ScoreData
{
    public List<Score> scores;

    public ScoreData()
    {
        scores = new List<Score>();
    }
}
