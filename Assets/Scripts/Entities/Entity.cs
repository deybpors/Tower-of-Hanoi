using UnityEngine;

public class Entity : MonoBehaviour
{
    public Renderer entityRenderer;
    [HideInInspector] public GameObject entityObject; 
    [HideInInspector] public Transform entityTransform;
    private InputManager _inputManager;

    public void Start()
    {
        entityObject = gameObject;
        entityTransform = transform;

        if (entityRenderer == null)
        {
            entityRenderer = GetComponent<Renderer>();
        }
        Manager.instance.EntitySubscribe(this, transform);
        _inputManager = Manager.instance.inputManager;
    }

    void Update()
    {
        if(!Manager.instance.started) return;

        if (Manager.instance.selectionManager.selected != entityTransform) return;

        if (Input.GetKeyDown(_inputManager.select))
        {
            OnSelectStart();
        }

        if (Input.GetKey(_inputManager.select))
        {
            OnSelect();
        }

        if (Input.GetKeyUp(_inputManager.select))
        {
            OnSelectEnd();
        }
    }

    public virtual void OnSelectStart(){}
    public virtual void OnSelect(){}
    public virtual void OnSelectEnd(){}
}
