using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public bool started;
    public bool active = true;
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

        _totalDuration += greatestDelay;
    }

    void Update()
    {
        if(!started) return;

        _timeElapsed += Time.deltaTime;

        if(_timeElapsed <= _totalDuration) return;

        if (!Input.GetKeyDown(KeyCode.Escape)) return;

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
    }

    public void Activate()
    {
        EnableAnims();
        Manager.instance.camController.enabled = false;
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
}
