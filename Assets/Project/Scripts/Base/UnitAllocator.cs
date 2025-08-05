using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAllocator : MonoBehaviour
{
    [SerializeField] private ResourceRegistry _resourceRegistry;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Base _basePrefab;

    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private int _resourcesNeededToSpawnUnit = 3;
    [SerializeField] private int _resourcesNeededToConstructBase = 5;
    [SerializeField] private bool spawnInitialUnits = false;

    private readonly List<Unit> _freeUnits = new List<Unit>();
    private readonly List<Unit> _allUnits = new List<Unit>();
    private readonly List<Resource> _deliveredResources = new List<Resource>();

    private Vector3? _constructionTarget;
    private int _resourcesCollectedForConstruction = 0;

    public event Action<Unit, Resource> ResourceDelivered;
    public event Action<int> ResourceSpent;

    public IReadOnlyList<Unit> FreeUnits => _freeUnits.AsReadOnly();

    private void Start()
    {
        if (spawnInitialUnits)
        {
            for (int i = 0; i < _initialUnitCount; i++)
            {
                SpawnUnit();
            }
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

    public void SetConstructionTarget(Vector3 targetPosition)
    {
        _constructionTarget = targetPosition;
        _resourcesCollectedForConstruction = _deliveredResources.Count;
    }

    public void RegisterExistingUnit(Unit unit)
    {
        unit.ResourceDelivered += HandleUnitResourceDelivered;
        _allUnits.Add(unit);
        _freeUnits.Add(unit);
    }

    private void HandleUnitResourceDelivered(Unit unit, Resource resource)
    {
        _deliveredResources.Add(resource);
        ResourceDelivered?.Invoke(unit, resource);
        _freeUnits.Add(unit);

        if (_constructionTarget.HasValue)
        {
            _resourcesCollectedForConstruction++;

            if (_resourcesCollectedForConstruction >= _resourcesNeededToConstructBase)
            {
                StartBaseConstruction();
            }
        }
        else
        {
            if (_deliveredResources.Count >= _resourcesNeededToSpawnUnit)
            {
                ProcessResourceConsumption(_resourcesNeededToSpawnUnit);
                SpawnUnit();
            }
        }
    }

    private void SpawnUnit()
    {
        Unit unit = _unitSpawner.Spawn();
        unit.ResourceDelivered += HandleUnitResourceDelivered;

        _freeUnits.Add(unit);
        _allUnits.Add(unit);
    }

    private void StartBaseConstruction()
    {
        if (_allUnits.Count <= 1 || _freeUnits.Count == 0)
        {
            return;
        }

        ProcessResourceConsumption(_resourcesNeededToConstructBase);

        Unit builder = _freeUnits[0];
        _freeUnits.RemoveAt(0);

        Vector3 target = _constructionTarget.Value;
        builder.GetComponent<UnitMover>().MoveTo(target, () => OnBuilderArrived(builder, target));

        _constructionTarget = null;
        _resourcesCollectedForConstruction = 0;
    }

    private void OnBuilderArrived(Unit builder, Vector3 buildPosition)
    {
        var flagPlacement = GetComponent<BaseFlagPlacement>();
        flagPlacement.ClearFlag();

        builder.ResourceDelivered -= HandleUnitResourceDelivered;
        _allUnits.Remove(builder);
        _freeUnits.Remove(builder);

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        Base nextBase = Instantiate(_basePrefab, buildPosition, rotation);

        UnitAllocator nextAllocator = nextBase.GetComponent<UnitAllocator>();
        nextAllocator.RegisterExistingUnit(builder);
    }

    private void ProcessResourceConsumption(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var returned = _deliveredResources[0];
            _deliveredResources.RemoveAt(0);
            returned.ReturnPool();
        }

        ResourceSpent?.Invoke(amount);
    }
}