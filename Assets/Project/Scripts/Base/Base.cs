using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SphereCollider _deliveryZone;
    [SerializeField] private ResourceScanner _resourceScanner;
    [SerializeField] private UnitAssignment _unitAssignment;
    [SerializeField] private UnitSpawnService _unitSpawnService;
    [SerializeField] private BaseConstructionService _baseConstructionService;

    private InputHandler _inputHandler;
    private BaseFlagPlacement _baseFlagPlacement;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _baseFlagPlacement = GetComponent<BaseFlagPlacement>();
    }

    private void OnEnable()
    {
        _inputHandler.ScanRequested += OnScanRequested;
        _baseFlagPlacement.FlagPlaced += OnFlagPlacement;
    }

    private void OnDisable()
    {
        _inputHandler.ScanRequested -= OnScanRequested;
        _baseFlagPlacement.FlagPlaced -= OnFlagPlacement;
    }

    private void OnScanRequested()
    {
        var foundResources = _resourceScanner.Scan(transform.position);
        _unitAssignment.AssignUnits(foundResources, _deliveryZone);
    }

    private void OnFlagPlacement(Vector3 flagPosition)
    {
        _baseConstructionService.SetConstructionTarget(flagPosition);
    }
}