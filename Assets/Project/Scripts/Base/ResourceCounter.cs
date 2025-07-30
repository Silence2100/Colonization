using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private UnitCoordinator _unitCoordinator;

    private int _count = 0;

    public event Action<int> ResourceCountChanged;

    private void Start()
    {
        ResourceCountChanged?.Invoke(_count);
    }

    private void OnEnable()
    {
        _unitCoordinator.UnitFreed += HandleUnitDelivered;
    }

    private void OnDisable()
    {
        _unitCoordinator.UnitFreed -= HandleUnitDelivered;
    }

    private void HandleUnitDelivered(Unit unit)
    {
        _count++;
        ResourceCountChanged?.Invoke(_count);
    }
}