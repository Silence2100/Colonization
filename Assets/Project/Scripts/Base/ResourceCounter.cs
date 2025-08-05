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
        _unitAllocator.ResourceDelivered += HandleResourceDelivered;
        _unitAllocator.ResourceSpent += HandleResourceSpent;
    }

    private void OnDisable()
    {
        _unitAllocator.ResourceDelivered -= HandleResourceDelivered;
        _unitAllocator.ResourceSpent -= HandleResourceSpent;
    }

    private void HandleResourceDelivered(Unit unit, Resource resource)
    {
        _count++;
        ResourceCountChanged?.Invoke(_count);
    }

    private void HandleResourceSpent(int spentAmount)
    {
        _count -= spentAmount;
        ResourceCountChanged?.Invoke(_count);
    }
}