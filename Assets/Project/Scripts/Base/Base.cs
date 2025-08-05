using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SphereCollider _deliveryZone;

    private InputHandler _input;
    private ResourceScanner _scanner;
    private UnitAllocator _unitAllocator;
    private BaseFlagPlacement _baseFlagPlacement;

    public static Base SelectedBase {  get; private set; }
    public static event Action<Base> BaseSelected;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _scanner = GetComponent<ResourceScanner>();
        _unitAllocator = GetComponent<UnitAllocator>();
        _baseFlagPlacement = GetComponent<BaseFlagPlacement>();
    }

    private void OnEnable()
    {
        _input.ScanRequested += HandleScan;
        _baseFlagPlacement.FlagPlaced += HandleFlagPlacement;
    }

    private void OnDisable()
    {
        _input.ScanRequested -= HandleScan;
        _baseFlagPlacement.FlagPlaced -= HandleFlagPlacement;
    }

    private void OnMouseDown()
    {
        SelectedBase = this;
        BaseSelected?.Invoke(this);
    }

    private void HandleScan()
    {
        if (SelectedBase != this)
        {
            return;
        }

        var found = _scanner.Scan(transform.position);
        _unitAllocator.AssignUnits(found, _deliveryZone);
    }

    private void HandleFlagPlacement(Vector3 flagPosition)
    {
        if (SelectedBase != this)
        {
            return;
        }

        _unitAllocator.SetConstructionTarget(flagPosition);
    }
}