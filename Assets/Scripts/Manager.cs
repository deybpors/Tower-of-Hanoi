using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool started;
    public int ringCount;
    public EntitySelectionManager selectionManager;
    public Stand stand;
    public CameraController camController;
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

        DontDestroyOnLoad(instance);
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
}
