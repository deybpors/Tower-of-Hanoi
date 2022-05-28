using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public bool active = true;
    [SerializeField] private ScoreboardUI _scoreboardUi;
    private GameObject _scoreboardUiObj;
    private UIAnimation[] _anims;
    private List<GameObject> _animationObj = new List<GameObject>();
    private float _timeElapsed;
    private float _totalDuration = 0f;

    void Awake()
    {
        _anims = GetComponentsInChildren<UIAnimation>();
        var count = _anims.Length;
        var greatestDelay = 0f;
        for (var i = 0; i < count; i++)
        {
            if (_totalDuration < _anims[i].duration)
            {
                _totalDuration = _anims[i].duration;
            }

            if (greatestDelay < _anims[i].delay)
            {
                greatestDelay = _anims[i].delay;
            }
        }

        _scoreboardUiObj = _scoreboardUi.gameObject;
        _totalDuration += greatestDelay;
        Manager.instance.menuUi = this;
    }

    void Update()
    {
        if(!Manager.instance.started) return;

        _timeElapsed += Time.deltaTime;

        if(_timeElapsed <= _totalDuration) return;

        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        ActiveDeactivate();
    }

    public void ActiveDeactivate()
    {
        active = !active;

        switch (active)
        {
            case false:
                Activate();
                break;
            case true:
                Deactivate();
                break;
        }

        _timeElapsed = 0;
    }

    public void Deactivate()
    {
        DisableAnims();
        Manager.instance.camController.enabled = true;
        Manager.instance.selectionManager.enabled = true;
        if (_scoreboardUiObj.activeInHierarchy)
        {
            _scoreboardUi.uiAnimation.Disable();
        }
    }

    private void Activate()
    {
        EnableAnims();
        Manager.instance.camController.enabled = false;
        Manager.instance.selectionManager.enabled = false;
        if (!Manager.instance.started)
        {
            _scoreboardUi.uiAnimation.Enable();
        }
    }

    private void EnableAnims()
    {
        if (_animationObj.Count <= 0)
        {
            var animCount = _anims.Length;
            for (var i = 0; i < animCount; i++)
            {
                try
                {
                    _animationObj.Add(_anims[i].gameObject);
                }
                catch
                {
                    //ignored
                }
            }
        }

        var count = _animationObj.Count;
        for (var i = 0; i < count; i++)
        {
            _animationObj[i].SetActive(true);
        }
    }

    private void DisableAnims()
    {
        var animCount = _anims.Length;
        for (var i = 0; i < animCount; i++)
        {
            _anims[i].Disable();
        }
    }
    public void Quit()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
