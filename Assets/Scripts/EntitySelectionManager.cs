using System.Collections.Generic;
using UnityEngine;

public class EntitySelectionManager : MonoBehaviour
{
    [SerializeField] private Material _highlightMaterial;
    public Transform selected;
    private Camera _camera;
    public Dictionary<Transform, Material> materials = new Dictionary<Transform, Material>();
    private Dictionary<Transform, Renderer> _renderers = new Dictionary<Transform, Renderer>();
    private Dictionary<RaycastHit, Transform> _hits = new Dictionary<RaycastHit, Transform>();
    private Transform _highlighted;

    void Start()
    {
        _camera = Manager.instance.camController.mainCam;
    }

    void Update()
    {
        ManageUnhighlight();

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var rayCast = Physics.Raycast(ray, out var hit);

        ManageHighlight(rayCast, hit);
        ManageSelection(rayCast);
    }

    private void ManageSelection(bool rayCast)
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

        if (!rayCast && selected != null)
        {
            if (!_renderers.TryGetValue(selected, out var selectedRenderer))
            {
                Manager.instance.IsInEntities(selected, out var entity);
                selectedRenderer = entity.entityRenderer;
                _renderers.Add(selected, selectedRenderer);
            }

            selectedRenderer.material = materials[selected];
            selected = null;
            return;
        }

        if (_highlighted == null) return;

        if (selected != _highlighted && selected != null)
        {
            if (!_renderers.TryGetValue(selected, out var selectedRenderer))
            {
                Manager.instance.IsInEntities(selected, out var entity);
                selectedRenderer = entity.entityRenderer;
                _renderers.Add(selected, selectedRenderer);
            }

            selectedRenderer.material = materials[selected];
        }

        selected = _highlighted;
    }

    private void ManageUnhighlight()
    {
        if (_highlighted == null) return;
        
        if (!_renderers.TryGetValue(_highlighted, out var highlightedRenderer))
        {
            Manager.instance.IsInEntities(_highlighted, out var entity);
            highlightedRenderer = entity.entityRenderer;
            _renderers.Add(_highlighted, highlightedRenderer);
        }

        if(highlightedRenderer == null) return;

        if(_highlighted == selected) return;

        highlightedRenderer.material = materials[_highlighted];
        _highlighted = null;
    }

    private void ManageHighlight(bool rayCast, RaycastHit hit)
    {
        if(!rayCast) return;

        if (!_hits.TryGetValue(hit, out var hitTransform))
        {
            hitTransform = hit.transform;
            _hits.Add(hit, hitTransform);
        }

        var highlighted = hitTransform;

        if (!Manager.instance.IsInEntities(highlighted, out var entity)) return;

        if (!_renderers.TryGetValue(highlighted, out var highlightedRenderer))
        {
            highlightedRenderer = entity.entityRenderer;
            _renderers.Add(highlighted, highlightedRenderer);
        }

        if (highlightedRenderer == null) return;
        
        if (!materials.TryGetValue(highlighted, out var material))
        {
            material = highlightedRenderer.material;
            materials.Add(highlighted, material);
        }

        highlightedRenderer.material = _highlightMaterial;
        _highlighted = highlighted;
    }

    public Transform GetEntitySelected()
    {
        return selected;
    }
}
