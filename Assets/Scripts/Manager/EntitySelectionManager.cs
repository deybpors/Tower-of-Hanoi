using System.Collections.Generic;
using UnityEngine;

public class EntitySelectionManager : MonoBehaviour
{
    [SerializeField] private Material _highlightMaterial;
    public Transform selected;
    private Camera _camera;
    private InputManager _inputManager;
    public Dictionary<Transform, Material> materials = new Dictionary<Transform, Material>();
    private Dictionary<Transform, Renderer> _renderers = new Dictionary<Transform, Renderer>();
    private Dictionary<RaycastHit, Transform> _hits = new Dictionary<RaycastHit, Transform>();
    private Transform _highlighted;

    void Start()
    {
        _camera = Manager.instance.camController.mainCam;
        _inputManager = Manager.instance.inputManager;
    }

    void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var rayCast = Physics.Raycast(ray, out var hit);

        ManageHighlight(rayCast, hit);
        ManageSelection(rayCast);
    }

    private void ManageHighlight(bool rayCast, RaycastHit hit)
    {
        if (!rayCast)
        {
            Unhighlight();
            _highlighted = null;
            return;
        }

        //if there is a casted object, check if it is an entity object
        if (!CheckCastedIfEntity(hit, out var highlighted, out var entity)) return;

        if (highlighted != _highlighted)
        {
            Unhighlight();
            Manager.instance.audioManager.PlaySfx("click", true, true);
        }

        Highlight(highlighted, entity);

        _highlighted = highlighted;
    }

    private void ManageSelection(bool rayCast)
    {
        if (!Input.GetKeyDown(_inputManager.select)) return;

        if (!rayCast)
        {
            //change material to original and make selected null
            if (selected == null) return;
            
            if (!_renderers.TryGetValue(selected, out var selectedRenderer))
            {
                Manager.instance.IsInEntities(selected, out var entity);
                selectedRenderer = entity.entityRenderer;
                _renderers.Add(selected, selectedRenderer);
            }

            selectedRenderer.material = materials[selected];
            selected = null;
        }
        else
        {
            //change material to original and make selected null
            if (selected == null)
            {
                selected = _highlighted;
                return;
            }

            if (selected == _highlighted) return;

            //unhighlight currently selected
            Manager.instance.IsInEntities(selected, out var entity);

            if (!_renderers.TryGetValue(selected, out var selectedRenderer))
            {
                selectedRenderer = entity.entityRenderer;
                _renderers.Add(selected, selectedRenderer);
            }

            selectedRenderer.material = materials[selected];
            selected = _highlighted;

            //highlight new selected
            Highlight(selected, entity);
        }
    }

    private void Unhighlight()
    {
        if (_highlighted == null || _highlighted == selected) return;

        Manager.instance.IsInEntities(_highlighted, out var entity);

        entity.entityRenderer.material = materials[_highlighted];
    }

    private void Highlight(Transform highlighted, Entity entity)
    {
        if (!_renderers.TryGetValue(highlighted, out var highlightedRenderer))
        {
            highlightedRenderer = entity.entityRenderer;
            _renderers.Add(highlighted, highlightedRenderer);
        }

        if (!materials.TryGetValue(highlighted, out var material))
        {
            material = highlightedRenderer.material;
            materials.Add(highlighted, material);
        }

        highlightedRenderer.material = _highlightMaterial;
    }

    private bool CheckCastedIfEntity(RaycastHit hit, out Transform highlighted, out Entity entity)
    {
        if (!_hits.TryGetValue(hit, out var hitTransform))
        {
            hitTransform = hit.transform;
            _hits.Add(hit, hitTransform);
        }
        highlighted = hitTransform;

        if (Manager.instance.IsInEntities(highlighted, out entity)) return true;
        Unhighlight();
        _highlighted = null;
        return false;
    }

}
