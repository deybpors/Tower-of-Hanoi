using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private List<ScoreboardRow> _rows;
    private int _rowsCount;
    public UIAnimation uiAnimation;
    private Dictionary<ScoreboardRow, Animator> _rowAnimators = new Dictionary<ScoreboardRow, Animator>();

    void Awake()
    {
        GetAnimators();
        _rowsCount = _rows.Count;
    }

    void OnEnable()
    {
        var scoreList = Manager.instance.scoreManager.SortScores();
        var scoreListCount = scoreList.Count;

        var inTop = false;

        for (var i = 0; i < _rowsCount; i++)
        {
            if (i >= scoreListCount)
            {
                _rows[i].gameObject.SetActive(false);
                continue;
            }
            _rows[i].PopulateRow(scoreList[i]);
            _rowAnimators[_rows[i]].Play("Normal");
            if (scoreList[i].playerName != Manager.instance.playerName) continue;
            
            inTop = true;
            _rowAnimators[_rows[i]].Play("Player");
        }

        var playerName = UpperCaseFirstChar(Manager.instance.playerName);
        if (inTop)
        {
            _winText.text = "Congratulations, " + playerName + "!";
        }
        else
        {
            _winText.text = "Better luck next time, " + playerName + ".";
        }
    }

    private string UpperCaseFirstChar(string text)
    {
        return Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
    }

    private void GetAnimators()
    {
        var rowCount = _rows.Count;
        for (var i = 0; i < rowCount; i++)
        {
            var row = _rows[i];
            _rowAnimators.Add(row, row.GetComponent<Animator>());
        }
    }
}
