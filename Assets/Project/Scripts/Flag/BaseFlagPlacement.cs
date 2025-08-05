using System;
using UnityEngine;

public class BaseFlagPlacement : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _layerMask;

    private Flag _currentFlag;
    private bool _isAwaitingPlacement = false;

    public event Action<Vector3> FlagPlaced;

    private void OnMouseUp()
    {
        _isAwaitingPlacement = true;
    }

    private void Update()
    {
        if (_isAwaitingPlacement == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                Vector3 targetPoint = hit.point;
                PlaceOrMoveFlag(targetPoint);
            }
            else
            {
                Debug.Log("Пошёл в жопу.");
            }

            _isAwaitingPlacement = false;
        }
    }

    public void ClearFlag()
    {
        if (_currentFlag != null)
        {
            Destroy(_currentFlag.gameObject);
            _currentFlag = null;
        }

        _isAwaitingPlacement = false;
    }

    private void PlaceOrMoveFlag(Vector3 target)
    {
        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab, target, Quaternion.identity);
        }
        else
        {
            _currentFlag.transform.position = target;
        }

        FlagPlaced?.Invoke(target);
    }
}