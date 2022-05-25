using UnityEngine;

public class Manager : MonoBehaviour
{
    public Stand stand;
    public CameraController camController;
    public static Manager instance;

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

        DontDestroyOnLoad(instance);
    }
}
