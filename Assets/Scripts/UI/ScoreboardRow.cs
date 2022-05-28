using System;
using TMPro;
using UnityEngine;

public class ScoreboardRow : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI time;
    public TextMeshProUGUI moves;
    public TextMeshProUGUI ringCount;
    private GameObject _rowObject;

    public void PopulateRow(Score score)
    {
        if (_rowObject == null)
        {
            _rowObject = gameObject;
        }
        _rowObject.SetActive(true);

        playerName.text = score.playerName.ToUpperInvariant();
        var timeSpan = TimeSpan.FromSeconds(score.time);
        time.text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
        moves.text = score.moves.ToString();
        ringCount.text = score.ringCount.ToString();
    }
}
