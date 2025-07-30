using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SphereCollider _deliveryZone;

    private InputHandler _input;
    private ResourceScanner _scanner;
    private UnitCoordinator _unitCoordinator;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        _scanner = GetComponent<ResourceScanner>();
        _unitCoordinator = GetComponent<UnitCoordinator>();
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
        _unitCoordinator.AssignUnits(found, _deliveryZone);
    }
}