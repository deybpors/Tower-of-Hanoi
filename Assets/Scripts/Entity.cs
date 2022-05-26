using UnityEngine;

public class Entity : MonoBehaviour
{
    public Renderer entityRenderer;
    [HideInInspector] public GameObject entityObject; 
    [HideInInspector] public Transform entityTransform; 

    public void Start()
    {
        entityObject = gameObject;
        entityTransform = transform;

        if (entityRenderer == null)
        {
            entityRenderer = GetComponent<Renderer>();
        }
        Manager.instance.EntitySubscribe(this, transform);
    }

    void Update()
    {
        if (Manager.instance.selectionManager.selected != entityTransform) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnSelectStart();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            OnSelect();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnSelectEnd();
        }
    }

    public virtual void OnSelectStart(){}
    public virtual void OnSelect(){}
    public virtual void OnSelectEnd(){}
}
