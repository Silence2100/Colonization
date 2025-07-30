using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitCoordinator : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _initialUnitCount = 3;

    private readonly List<Unit> _freeUnits = new List<Unit>();
    private readonly List<Unit> _allUnits = new List<Unit>();

    public event Action<Unit> UnitFreed;

    public IReadOnlyList<Unit> FreeUnits => _freeUnits;

    private void Start()
    {
        for (int i = 0; i < _initialUnitCount; i++)
        {
            Unit unit = _unitSpawner.SpawnOne();
            unit.ResourceDelivered += HandleUnitDelivered;

            _freeUnits.Add(unit);
            _allUnits.Add(unit);
        }
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.ResourceDelivered -= HandleUnitDelivered;
        }
    }

    private void HandleUnitDelivered(Unit unit)
    {
        _freeUnits.Add(unit);
        UnitFreed?.Invoke(unit);
    }

    public void AssignUnits(List<Resource> resources, SphereCollider deliveryZone)
    {
        while (_freeUnits.Count > 0 && resources.Count > 0)
        {
            Unit unit = _freeUnits[0];
            Resource resource = resources[0];

            _freeUnits.RemoveAt(0);
            resources.RemoveAt(0);

            resource.Reserve();
            unit.SetTarget(resource, deliveryZone);
        }
    }
}