using System;
using UnityEngine;

public class BaseFlagPlacement : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private Flag _currentFlag;

    public event Action<Vector3> FlagPlaced;

    public void PlaceOrMoveFlag(Vector3 target)
    {
        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab, target, Quaternion.identity);
        }
        else
        {
            _currentFlag.transform.position = target;
            _currentFlag.gameObject.SetActive(true);
        }

        FlagPlaced?.Invoke(target);
    }

    public void ClearFlag()
    {
        if (_currentFlag != null)
        {
            _currentFlag.gameObject?.SetActive(false);
        }
    }
}