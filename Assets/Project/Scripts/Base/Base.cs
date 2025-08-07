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

    private static Base _selectedBase;

    public static event Action<Base> BaseSelected;

    public static Base SelectedBase => _selectedBase;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _baseFlagPlacement = GetComponent<BaseFlagPlacement>();
    }

    private void OnEnable()
    {
        _inputHandler.ScanRequested += HandleScanRequested;
        _baseFlagPlacement.FlagPlaced += HandleFlagPlacement;
    }

    private void OnDisable()
    {
        _inputHandler.ScanRequested -= HandleScanRequested;
        _baseFlagPlacement.FlagPlaced -= HandleFlagPlacement;
    }

    private void OnMouseDown()
    {
        _selectedBase = this;
        BaseSelected?.Invoke(this);
    }

    private void HandleScanRequested()
    {
        if (SelectedBase != this)
        {
            return;
        }

        var foundResources = _resourceScanner.Scan(transform.position);
        _unitAssignment.AssignUnits(foundResources, _deliveryZone);
    }

    private void HandleFlagPlacement(Vector3 flagPosition)
    {
        if (SelectedBase != this)
        {
            return;
        }

        _baseConstructionService.SetConstructionTarget(flagPosition);
    }
}