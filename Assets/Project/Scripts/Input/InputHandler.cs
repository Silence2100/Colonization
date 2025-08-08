using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const KeyCode ScanKey = KeyCode.S;

    [SerializeField] private BaseSelectionService _baseSelectionService;
    [SerializeField] private LayerMask _flagPlacementLayerMask;

    private Base _ownBase;
    private BaseFlagPlacement _flagPlacement;

    public event Action ScanRequested;

    private void Awake()
    {
        _ownBase = GetComponent<Base>();
        _flagPlacement = GetComponent<BaseFlagPlacement>();
    }

    private void Update()
    {
        HandleScanInput();
        HandleFlagInput();
    }

    private void HandleScanInput()
    {
        if (Input.GetKeyDown(ScanKey) && _baseSelectionService.SelectedBase == _ownBase)
        {
            ScanRequested?.Invoke();
        }
    }

    private void HandleFlagInput()
    {
        if (Input.GetMouseButtonDown(1) && _baseSelectionService.SelectedBase == _ownBase)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _flagPlacementLayerMask))
            {
                _flagPlacement.PlaceOrMoveFlag(hit.point);
            }
        }
    }

    private void OnMouseDown()
    {
        _baseSelectionService.SelectBase(_ownBase);
    }
}