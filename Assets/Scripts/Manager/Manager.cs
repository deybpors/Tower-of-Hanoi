using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public bool started;
    public EntitySelectionManager selectionManager;
    public Stand stand;
    public CameraController camController;
    public ScoreManager scoreManager;
    public InputManager inputManager;
    public AudioManager audioManager;
    [HideInInspector] public MenuUI menuUi;
    [HideInInspector] public int ringCount;
    [HideInInspector] public Stopwatch stopwatch;
    [HideInInspector] public MoveCounter moveCounter;
    [HideInInspector] public string playerName;
    public static Manager instance;

    private Dictionary<Transform, Entity> _entities = new Dictionary<Transform, Entity>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        DontDestroyOnLoad(instance);
    }
    void Start()
    {
        var musicsList = audioManager.musics;
        var music = musicsList[Random.Range(0, musicsList.Count)];
        audioManager.PlayMusic(music.audioName, true);
    }

    public void EntitySubscribe(Entity entity, Transform trans)
    {
        _entities.TryAdd(trans, entity);
    }

    public void EntityUnsubscribe(Transform trans)
    {
        _entities.Remove(trans);
    }

    public bool IsInEntities(Transform trans, out Entity entity)
    {
        return _entities.TryGetValue(trans, out entity);
    }

    public void EndCheck()
    {
        if (stand.GetLastPeg().rings.Count != ringCount) return;
        
        started = false;
        stopwatch.StopStopwatch();
        scoreManager.AddScore(new Score(playerName, stopwatch.GetTime(), moveCounter.AddMoves(0), ringCount));
        menuUi.ActiveDeactivate();
        audioManager.PlaySfx("confirmation", true, true);
        stopwatch.uiAnimation.Disable();
        moveCounter.uiAnimation.Disable();
    }
}
