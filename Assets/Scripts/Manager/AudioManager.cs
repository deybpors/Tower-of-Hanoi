using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    [Space]
    public float fadeTime = 1f;

    private float _timeElapsed;
    public List<Music> musics;
    private Music _currentMusic;
    public List<Sfx> sfx;
    public Dictionary<string, Audio> audioDictionary = new Dictionary<string, Audio>();
    private GameObject _thisObject;

    void Awake()
    {
        _thisObject = gameObject;
        SetVolume();
        SetAudios();
    }

    void Update()
    {
        if(_currentMusic == null || _currentMusic.source == null) return;
        _timeElapsed += Time.unscaledDeltaTime;

        if (_timeElapsed < _currentMusic.clip.length - fadeTime) return;
        
        var music = musics[Random.Range(0, musics.Count)];
        while (music == _currentMusic)
        {
            music = musics[Random.Range(0, musics.Count)];
        }

        PlayMusic(music.audioName, true);

        _timeElapsed = 0;
    }

    private void SetAudios()
    {
        foreach (var music in musics)
        {
            music.audioName = music.clip.name;
            music.source = _thisObject.AddComponent<AudioSource>();
            music.source.clip = music.clip;
            music.source.loop = music.loop;
            music.source.volume = music.volume * masterVolume * musicVolume;
            audioDictionary.TryAdd(music.audioName.ToLowerInvariant().Replace(" ", string.Empty), music);
        }

        foreach (var fx in sfx)
        {
            fx.audioName = fx.clip.name;
            fx.source = _thisObject.AddComponent<AudioSource>();
            fx.source.clip = fx.clip;
            fx.source.volume = fx.volume * masterVolume * sfxVolume;
            fx.source.pitch = fx.defaultPitch;
            audioDictionary.TryAdd(fx.audioName.ToLowerInvariant().Replace(" ", string.Empty), fx);
        }
    }

    private void SetVolume()
    {
        masterVolume = PlayerPrefs.GetFloat("master", 1f);
        musicVolume = PlayerPrefs.GetFloat("music", 1f);
        sfxVolume = PlayerPrefs.GetFloat("sfx", 1f);
    }

    void OnDestroy()
    {
        SaveVolume();
    }

    public void PlayMusic(string audioName, bool fade)
    {
        audioName = audioName.ToLowerInvariant().Replace(" ", string.Empty);
        if (!audioDictionary.TryGetValue(audioName, out var music)) return;

        var musicToPlay = (Music) music;
        if (fade)
        {
            StopAllCoroutines();
            StartCoroutine(FadeTrack(musicToPlay));
        }
        else
        {
            if (_currentMusic != null && _currentMusic.source != null)
            {
                _currentMusic.source.Stop();
            }

            musicToPlay.source.Play();
            _currentMusic = musicToPlay;
        }
    }

    public void PlaySfx(string audioName, bool oneShot, bool randomPitch)
    {
        audioName = audioName.ToLowerInvariant().Replace(" ", string.Empty);
        if (!audioDictionary.TryGetValue(audioName, out var fx)) return;
        
        var fxToPlay = (Sfx) fx;
        if (oneShot)
        {
            if (randomPitch)
            {
                var pitch = Random.Range(fxToPlay.minPitch, fxToPlay.maxPitch);
                fx.source.pitch = pitch;
            }
            else
            {
                fx.source.pitch = fxToPlay.defaultPitch;
            }

            fx.source.PlayOneShot(fx.source.clip);
        }
        else
        {
            if (randomPitch)
            {
                var pitch = Random.Range(fxToPlay.minPitch, fxToPlay.maxPitch);
                fx.source.pitch = pitch;
            }
            else
            {
                fx.source.pitch = fxToPlay.defaultPitch;
            }
            
            fx.source.Play();
        }
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("master", masterVolume);
        PlayerPrefs.SetFloat("music", musicVolume);
        PlayerPrefs.SetFloat("sfx", sfxVolume);
    }

    private IEnumerator FadeTrack(Music music)
    {
        var timeElapsed = 0f;

        music.source.Play();
        music.source.volume = 0;
        var targetVolume = music.volume * musicVolume * masterVolume;
        var notNull = _currentMusic != null && _currentMusic.source != null;

        while (timeElapsed < fadeTime)
        {
            music.source.volume = Mathf.Lerp(music.source.volume, targetVolume, timeElapsed / fadeTime);
            
            if (notNull)
            {
                _currentMusic.source.volume = Mathf.Lerp(_currentMusic.source.volume, 0f, timeElapsed / fadeTime);
            }

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (notNull && _currentMusic.source.volume != 0f)
        {
            _currentMusic.source.volume = 0f;
            _currentMusic.source.Stop();
        }

        _currentMusic = music;
    }
}
