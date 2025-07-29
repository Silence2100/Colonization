using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private ResourceScanner _scanner;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private SphereCollider _deliveryZone;
    [SerializeField] private int _initialUnitCount = 3;

    private readonly List<Unit> _freeUnits = new List<Unit>();
    private readonly List<Unit> _allUnits = new List<Unit>();

    private int _resourceCount = 0;

    public event Action<int> OnResourceCountChanged;

    private void Start()
    {
        OnResourceCountChanged?.Invoke(_resourceCount);

        for (int i = 0; i < _initialUnitCount; i++)
        {
            Unit unit = _unitSpawner.SpawnOne();
            unit.OnResourceDelivered += HandleUnitDelivered;

            _freeUnits.Add(unit);
            _allUnits.Add(unit);
        }
    }

    private void OnEnable()
    {
        _inputHandler.OnScanRequested += HandleScanRequested;
    }

    private void OnDisable()
    {
        _inputHandler.OnScanRequested -= HandleScanRequested;
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _allUnits)
        {
            unit.OnResourceDelivered -= HandleUnitDelivered;
        }
    }

    private void HandleScanRequested()
    {
        var found = _scanner.Scan(transform.position);
        AssignUnitsToResources(found);
    }

    private void AssignUnitsToResources(List<Resource> resources)
    {
        while (_freeUnits.Count > 0 && resources.Count > 0)
        {
            Unit unit = _freeUnits[0];
            Resource resource = resources[0];

            resource.Reserve();

            _freeUnits.RemoveAt(0);
            resources.RemoveAt(0);

            unit.SetTarget(resource, _deliveryZone);
        }
    }

    private void HandleUnitDelivered(Unit unit)
    {
        _resourceCount++;
        OnResourceCountChanged?.Invoke(_resourceCount);

        _freeUnits.Add(unit);
    }
}