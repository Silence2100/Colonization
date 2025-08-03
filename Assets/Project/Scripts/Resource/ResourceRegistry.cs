using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRegistry : MonoBehaviour
{
    private readonly List<Resource> _freeResources = new List<Resource>();
    private readonly List<Resource> _reservedResources = new List<Resource>();

    public void RegisterResource(Resource resource)
    {
        if (_freeResources.Contains(resource) || _reservedResources.Contains(resource))
        {
            return;
        }

        _freeResources.Add(resource);
        resource.ReturnToPool += HandleResourceReturned;
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

    private void HandleResourceReturned(Resource resource)
    {
        _freeResources.Remove(resource);
        _reservedResources.Remove(resource);
        resource.ReturnToPool -= HandleResourceReturned;
    }
}