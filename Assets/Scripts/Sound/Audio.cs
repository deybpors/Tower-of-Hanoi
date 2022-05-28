using System;
using UnityEngine;

[Serializable]
public class Audio
{
    [HideInInspector] public string audioName;
    public AudioSource source;
    public AudioClip clip;
    public float volume = 1f;
}

[Serializable]
public class Music : Audio
{
    public bool loop;
}

[Serializable]
public class Sfx : Audio
{
    public float defaultPitch = 1f;
    public float minPitch;
    public float maxPitch;
}
