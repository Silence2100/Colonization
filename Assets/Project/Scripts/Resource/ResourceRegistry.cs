using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRegistry : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private readonly List<Resource> _freeResources = new List<Resource>();
    private readonly List<Resource> _reservedResources = new List<Resource>();

    private void OnEnable()
    {
        _resourceSpawner.ResourceSpawned += HandleResourceSpawned;
    }

    private void OnDisable()
    {
        _resourceSpawner.ResourceSpawned -= HandleResourceSpawned;
    }

    private void HandleResourceSpawned(Resource resource)
    {
        _freeResources.Add(resource);
        resource.ReturnToPool += HandleResourceReturned;
    }

    private void HandleResourceReturned(Resource resource)
    {
        _freeResources.Remove(resource);
        _reservedResources.Remove(resource);
        resource.ReturnToPool -= HandleResourceReturned;
    }

    public void ReserveResource(Resource resource)
    {
        if (_freeResources.Remove(resource))
        {
            _reservedResources.Add(resource);
        }
    }

    public bool IsResourceFree(Resource resource)
    {
        return _freeResources.Contains(resource);
    }

    public IReadOnlyList<Resource> GetFreeResources()
    {
        return _freeResources.AsReadOnly();
    }
}