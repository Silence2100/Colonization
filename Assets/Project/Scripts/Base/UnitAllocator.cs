using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAllocator : MonoBehaviour
{
    [SerializeField] private ResourceRegistry _resourceRegistry;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private int _resourcesNeededToSpawnUnit = 3;

    private readonly List<Unit> _freeUnits = new List<Unit>();
    private readonly List<Unit> _allUnits = new List<Unit>();
    private readonly List<Resource> _deliveredResources = new List<Resource>();

    private int _resourcesCollectedSinceLastSpawn = 0;

    public event Action<Unit, Resource> ResourceDelivered;
    public event Action<int> ResourceSpent;

    public IReadOnlyList<Unit> FreeUnits => _freeUnits.AsReadOnly();

    private void Start()
    {
        for (int i = 0; i < _initialUnitCount; i++)
        {
            SpawnUnit();
        }
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.ResourceDelivered -= HandleUnitResourceDelivered;
        }
    }

    public void AssignUnits(List<Resource> resources, SphereCollider deliveryZone)
    {
        while (_freeUnits.Count > 0 && resources.Count > 0)
        {
            Unit unit = _freeUnits[0];
            Resource resource = resources[0];

            _freeUnits.RemoveAt(0);
            resources.RemoveAt(0);

            _resourceRegistry.ReserveResource(resource);
            unit.SetTarget(resource, deliveryZone);
        }
    }

    private void HandleUnitResourceDelivered(Unit unit, Resource resource)
    {
        _deliveredResources.Add(resource);

        ResourceDelivered?.Invoke(unit, resource);

        _resourcesCollectedSinceLastSpawn++;

        _freeUnits.Add(unit);

        if (_deliveredResources.Count >= _resourcesNeededToSpawnUnit)
        {
            for (int i = 0; i < _resourcesCollectedSinceLastSpawn; i++)
            {
                Resource returnedResource = _deliveredResources[0];
                _deliveredResources.RemoveAt(0);
                returnedResource.ReturnPool();
            }

            ResourceSpent?.Invoke(_resourcesNeededToSpawnUnit);

            SpawnUnit();
            _resourcesCollectedSinceLastSpawn -= _resourcesNeededToSpawnUnit;
        }
    }

    private void SpawnUnit()
    {
        Unit unit = _unitSpawner.Spawn();
        unit.ResourceDelivered += HandleUnitResourceDelivered;

        _freeUnits.Add(unit);
        _allUnits.Add(unit);
    }
}