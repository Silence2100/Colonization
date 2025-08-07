using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private UnitAssignment _unitAssignment;

    private int _count = 0;

    public int CurrentCount => _count;

    public event Action<int> ResourceCountChanged;

    private void Start()
    {
        ResourceCountChanged?.Invoke(_count);
    }

    private void OnEnable()
    {
        _unitAssignment.ResourceDelivered += HandleResourceDelivered;
        _unitAssignment.ResourcesSpent += HandleResourceSpent;
    }

    private void OnDisable()
    {
        _unitAssignment.ResourceDelivered -= HandleResourceDelivered;
        _unitAssignment.ResourcesSpent -= HandleResourceSpent;
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