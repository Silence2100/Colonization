using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private UnitCoordinator _unitCoordinator;

    private int _count = 0;

    public event Action<int> OnResourceCountChanged;

    private void Start()
    {
        OnResourceCountChanged?.Invoke(_count);
    }

    private void OnEnable()
    {
        _unitCoordinator.OnUnitFreed += HandleUnitDelivered;
    }

    private void OnDisable()
    {
        _unitCoordinator.OnUnitFreed -= HandleUnitDelivered;
    }

    private void HandleUnitDelivered(Unit unit)
    {
        _count++;
        OnResourceCountChanged?.Invoke(_count);
    }
}