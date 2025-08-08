using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const KeyCode ScanKey = KeyCode.S;

    [SerializeField] private BaseSelectionService _baseSelectionService;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _baseLayerMask;
    [SerializeField] private LayerMask _flagPlacementLayerMask;

    private Base _selectedBase;
    private BaseFlagPlacement _flagPlacementComponent;

    private void Awake()
    {
        _baseSelectionService.BaseSelected += OnBaseSelected;

        if (_baseSelectionService.SelectedBase != null)
        {
            OnBaseSelected(_baseSelectionService.SelectedBase);
        }
    }

    private void OnDestroy()
    {
        _baseSelectionService.BaseSelected -= OnBaseSelected;
    }

    private void Update()
    {
        HandleSelection();
        HandleScan();
        HandleFlagPlacement();
    }

    private void OnBaseSelected(Base nextBase)
    {
        _selectedBase = nextBase;
        _flagPlacementComponent = nextBase != null ? nextBase.GetComponent<BaseFlagPlacement>() : null;
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0) == false)
        {
            return;
        }

        if (TryRaycast(_baseLayerMask, out var hit))
        {
            Base baseUnderCursor = hit.collider.GetComponentInParent<Base>();

            if (baseUnderCursor != null)
            {
                _baseSelectionService.SelectBase(baseUnderCursor);
            }
        }
    }

    private void HandleScan()
    {
        if (Input.GetKeyDown(ScanKey) && _selectedBase != null)
        {
            _selectedBase.Scan();
        }
    }

    private void HandleFlagPlacement()
    {
        if (Input.GetMouseButtonDown(1) == false || _flagPlacementComponent == null)
        {
            return;
        }

        if (TryRaycast(_flagPlacementLayerMask, out var hit))
        {
            _flagPlacementComponent.PlaceOrMoveFlag(hit.point);
        }
    }

    private bool TryRaycast(LayerMask mask, out RaycastHit hit)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
    }
}