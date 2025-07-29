using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Vector3 _carryOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] private float _dropHeight = 2f;

    private Resource _targetResource;
    private SphereCollider _deliveryZone;
    private UnitMover _mover;

    public event Action<Unit> OnResourceDelivered;

    private void Awake()
    {
        _mover = GetComponent<UnitMover>();
    }

    public void SetTarget(Resource resource, SphereCollider deliveryZone)
    {
        _targetResource = resource;
        _deliveryZone = deliveryZone;

        _mover.MoveTo(_targetResource.transform.position, OnArrivedAtResource);
    }

    private void OnArrivedAtResource()
    {
        _targetResource.Collect();

        _targetResource.transform.SetParent(transform);
        _targetResource.transform.localPosition = _carryOffset;

        Vector3 dropPoint = _deliveryZone.ClosestPoint(transform.position);
        _mover.MoveTo(dropPoint, OnArrivedBase);
    }

    private void OnArrivedBase()
    {
        _targetResource.transform.SetParent(null);

        float worldRadius = (_deliveryZone.radius - 0.2f) * _deliveryZone.transform.localScale.x;

        Vector3 randomCircle = UnityEngine.Random.insideUnitCircle * worldRadius;
        Vector3 dropPosition = _deliveryZone.transform.position + new Vector3(randomCircle.x, _dropHeight, randomCircle.y);
        _targetResource.transform.position = dropPosition;

        if (_targetResource.TryGetComponent<BoxCollider>(out var collision))
        {
            collision.enabled = true;
            collision.isTrigger = false;
        }

        if (_targetResource.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }

        OnResourceDelivered?.Invoke(this);
        _targetResource = null;
    }
}