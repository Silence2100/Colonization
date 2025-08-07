using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const KeyCode ScanKey = KeyCode.S;

    [SerializeField] private LayerMask _flagPlacementLayerMask;

    private Base _baseComponent;
    private BaseFlagPlacement _flagPlacement;

    public event Action ScanRequested;

    private void Awake()
    {
        _baseComponent = GetComponent<Base>();
        _flagPlacement = GetComponent<BaseFlagPlacement>();
    }

    private void Update()
    {
        HandleScanInput();
        HandleFlagPlacementInput();
    }

    private void HandleScanInput()
    {
        if (Input.GetKeyDown(ScanKey) && Base.SelectedBase == _baseComponent)
        {
            ScanRequested?.Invoke();
        }
    }

    private void HandleFlagPlacementInput()
    {
        if (Input.GetMouseButtonDown(1) && Base.SelectedBase == _baseComponent)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _flagPlacementLayerMask))
            {
                _flagPlacement.PlaceOrMoveFlag(hit.point);
            }
        }
    }
}