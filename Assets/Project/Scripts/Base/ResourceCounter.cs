using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private UnitAllocator _unitAllocator;

    private int _count = 0;

    public event Action<int> ResourceCountChanged;

    private void Start()
    {
        ResourceCountChanged?.Invoke(_count);
    }

    private void OnEnable()
    {
        _unitAllocator.UnitFreed += HandleUnitDelivered;
    }

    private void OnDisable()
    {
        _unitAllocator.UnitFreed -= HandleUnitDelivered;
    }

    private void HandleUnitDelivered(Unit unit)
    {
        _count++;
        ResourceCountChanged?.Invoke(_count);
    }
}