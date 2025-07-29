using System;
using System.Collections;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _arrivalThreshold = 0.1f;

    private float _arrivalThresholdSqr;

    private void Awake()
    {
        _arrivalThresholdSqr = _arrivalThreshold * _arrivalThreshold;
    }

    public void MoveTo(Vector3 destination, Action onArrived)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(destination, onArrived));
    }

    private IEnumerator MoveRoutine(Vector3 destination, Action onArrived)
    {
        while ((transform.position - destination).sqrMagnitude > _arrivalThresholdSqr)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                _moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        onArrived?.Invoke();
    }
}