using System;
using UnityEngine;

public class ResourceCarrier : MonoBehaviour
{
    [SerializeField] private float _carryHeight = 1.0f;
    [SerializeField] private float _dropHeight = 2f;
    [SerializeField] private float _zoneMargin = 0.2f;

    private Resource _currentResource;
    private SphereCollider _deliveryZone;
    private UnitMover _mover;

    public event Action Delivered;

    private void Awake()
    {
        _mover = GetComponent<UnitMover>();
    }

    public void CollectAndDeliver(Resource resource, SphereCollider zone)
    {
        _currentResource = resource;
        _deliveryZone = zone;
        _mover.MoveTo(resource.transform.position, OnArrivedAtResource);
    }

    private void OnArrivedAtResource()
    {
        _currentResource.Collect();

        _currentResource.transform.SetParent(transform);
        _currentResource.transform.localPosition = Vector3.up * _carryHeight;

        Vector3 dropPoint = _deliveryZone.ClosestPoint(transform.position);
        _mover.MoveTo(dropPoint, OnArrivedAtZone);
    }

    private void OnArrivedAtZone()
    {
        _currentResource.transform.SetParent(null);
        _currentResource.transform.position = CalculateDropPoint();

        if (_currentResource.TryGetComponent<BoxCollider>(out var collision))
        {
            collision.enabled = true;
            collision.isTrigger = false;
        }

        if (_currentResource.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }

        Delivered?.Invoke();
        _currentResource = null;
    }

    private Vector3 CalculateDropPoint()
    {
        float worldRadius = (_deliveryZone.radius - _zoneMargin) * _deliveryZone.transform.localScale.x;

        Vector3 randomCircle = UnityEngine.Random.insideUnitCircle * worldRadius;
        Vector3 dropPosition = _deliveryZone.transform.position + new Vector3(randomCircle.x, _dropHeight, randomCircle.y);

        return dropPosition;
    }
}