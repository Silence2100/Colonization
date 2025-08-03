using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SphereCollider _deliveryZone;

    private InputHandler _input;
    private ResourceScanner _scanner;
    private UnitAllocator _unitAllocator;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _scanner = GetComponent<ResourceScanner>();
        _unitAllocator = GetComponent<UnitAllocator>();
    }

    private void OnEnable()
    {
        _input.ScanRequested += HandleScan;
    }

    private void OnDisable()
    {
        _input.ScanRequested -= HandleScan;
    }

    private void HandleScan()
    {
        var found = _scanner.Scan(transform.position);
        _unitAllocator.AssignUnits(found, _deliveryZone);
    }
}