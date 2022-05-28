using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreData _scoreData;

    void Start()
    {
        var json = PlayerPrefs.GetString("scores", "{}");
        _scoreData = JsonUtility.FromJson<ScoreData>(json);
    }

    public List<Score> SortScores()
    {
        var data = _scoreData.scores
            .OrderByDescending(x => x.ringCount)
            .ThenBy(x => x.moves)
            .ThenBy(x => x.time)
            .ToList();
        
        var dataCount = data.Count;
        _scoreData.scores.Clear();

        for (var i = 0; i < dataCount; i++)
        {
            _scoreData.scores.Add(data[i]);
        }

        return _scoreData.scores;
    }

    public void AddScore(Score score)
    {
        var scoreCount = _scoreData.scores.Count;
        for (var i = 0; i < scoreCount; i++)
        {
            var scoreData = _scoreData.scores[i];
            if (scoreData.playerName != score.playerName) continue;

            scoreData.moves = score.moves;
            scoreData.time = score.time;
            return;
        }

        _scoreData.scores.Add(score);
    }

    public void OnDestroy()
    {
        SaveScore();
    }

    private void SaveScore()
    {
        var json = JsonUtility.ToJson(_scoreData);
        PlayerPrefs.SetString("scores", json);
    }
}
