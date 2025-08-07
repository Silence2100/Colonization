using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitAssignment : MonoBehaviour
{
    [SerializeField] private ResourceRegistry _resourceRegistry;

    private readonly List<Unit> _freeUnits = new List<Unit>();
    private readonly List<Resource> _delivered = new List<Resource>();

    public event Action<Unit, Resource> ResourceDelivered;
    public event Action<int> ResourcesSpent;

    public IReadOnlyList<Unit> FreeUnits => _freeUnits.AsReadOnly();

    public int CurrentDeliveredCount() => _delivered.Count;

    public void RegisterUnit(Unit unit)
    {
        unit.ResourceDelivered += OnUnitDelivered;
        _freeUnits.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        unit.ResourceDelivered -= OnUnitDelivered;
        _freeUnits.Remove(unit);
    }

    public void AssignUnits(List<Resource> resources, SphereCollider deliveryZone)
    {
        while (_freeUnits.Count > 0 && resources.Count > 0)
        {
            var unit = _freeUnits[0];
            var resource = resources[0];

            _freeUnits.RemoveAt(0);
            resources.RemoveAt(0);

            _resourceRegistry.ReserveResource(resource);
            unit.SetTarget(resource, deliveryZone);
        }
    }

    public void SpendResources(int amount)
    {
        int actualSpent = 0;

        for (int i = 0; i < amount && _delivered.Count > 0; i++)
        {
            var returned = _delivered[0];
            _delivered.RemoveAt(0);
            returned.ReturnPool();
            actualSpent++;
        }

        ResourcesSpent?.Invoke(actualSpent);
    }

    public void OnUnitDelivered(Unit unit, Resource resource)
    {
        _delivered.Add(resource);
        ResourceDelivered?.Invoke(unit, resource);
        _freeUnits.Add(unit);
    }
}