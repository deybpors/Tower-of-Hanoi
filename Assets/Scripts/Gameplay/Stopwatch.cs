using System;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    private bool _active;
    private float _currentTime;
    public UIAnimation uiAnimation;
    public TextMeshProUGUI currentTimeText;
    void Start()
    {
        Manager.instance.stopwatch = this;
    }
    void OnEnable()
    {
        _currentTime = 0;
        StartStopwatch();
        uiAnimation.enabled = true;
    }

    void OnDisable()
    {
        uiAnimation.enabled = false;
       StopStopwatch();
    }

    void Update()
    {
        if (_active)
        {
            _currentTime += Time.deltaTime;
        }

        var time = TimeSpan.FromSeconds(_currentTime);
        currentTimeText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
    }

    public float GetTime()
    {
        return _currentTime;
    }

    public void StopStopwatch()
    {
        _active = false;
    }

    private void StartStopwatch()
    {
        _active = true;
    }

}
