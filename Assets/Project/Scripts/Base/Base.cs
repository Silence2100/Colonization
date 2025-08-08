using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SphereCollider _deliveryZone;
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private UnitAssignment _unitAssignment;
    [SerializeField] private UnitSpawnService _unitSpawnService;
    [SerializeField] private BaseConstructionService _baseConstructionService;
    [SerializeField] private BaseFlagPlacement _baseFlagPlacement;

    private int _resourceCount = 0;

    public int CurrentResourceCount => _resourceCount;

    public event Action<int> ResourceCountChanged; 

    private void OnEnable()
    {
        _baseFlagPlacement.FlagPlaced += OnFlagPlacement;
        _unitAssignment.ResourceDelivered += OnUnitDelivered;
        _unitAssignment.ResourcesSpent += OnResourceSpent;
    }

    private void OnDisable()
    {
        _baseFlagPlacement.FlagPlaced -= OnFlagPlacement;
        _unitAssignment.ResourceDelivered -= OnUnitDelivered;
        _unitAssignment.ResourcesSpent -= OnResourceSpent;
    }

    public void Scan()
    {
        var foundResources = _resourceScanner.Scan(transform.position);
        _unitAssignment.AssignUnits(foundResources, _deliveryZone);
    }

    private void OnFlagPlacement(Vector3 flagPosition)
    {
        _baseConstructionService.SetConstructionTarget(flagPosition);
    }

    private void OnUnitDelivered(Unit unit, Resource resource)
    {
        _resourceCount++;
        ResourceCountChanged?.Invoke(_resourceCount);

        _unitSpawnService.OnResourceDelivered(_resourceCount);
        _baseConstructionService.OnResourceDelivered(_resourceCount);
    }

    private void OnResourceSpent(int amount)
    {
        _resourceCount -= amount;
        ResourceCountChanged?.Invoke(_resourceCount);
    }
}