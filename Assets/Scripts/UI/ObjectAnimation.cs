using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimation : MonoBehaviour
{
    public float duration;
    public float delay;

    [HideInInspector] public Coroutine coroutine;
    [HideInInspector] public Transform transformObject;

    void Awake()
    {
        transformObject = GetComponent<Transform>();
    }

    public bool AnimationDone()
    {
        return coroutine == null;
    }
}
