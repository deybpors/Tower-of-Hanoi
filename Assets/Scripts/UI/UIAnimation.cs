using System.Collections;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public float duration;
    public float delay;
    
    [HideInInspector] public Coroutine coroutine;
    [HideInInspector] public RectTransform uiObject;

    void Awake()
    {
        uiObject = GetComponent<RectTransform>();
    }
    public virtual void Disable(){}
    public bool AnimationDone()
    {
        return coroutine == null;
    }
}
