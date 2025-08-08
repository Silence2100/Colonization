using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private ResourceCarrier _carrier;
    private UnitMover _unitMover;

    public event Action<Unit, Resource> ResourceDelivered;

    private void Awake()
    {
        _carrier = GetComponent<ResourceCarrier>();
        _unitMover = GetComponent<UnitMover>();
    }

    private void OnEnable()
    {
        _carrier.Delivered += HandleCarrierDelivered;
    }

    private void OnDisable()
    {
        _carrier.Delivered += HandleCarrierDelivered;
    }

    public void SetTarget(Resource resource, SphereCollider deliveryZone)
    {
        _carrier.CollectAndDeliver(resource, deliveryZone);
    }

    public void MoveToPosition(Vector3 destination, Action onArrived)
    {
        _unitMover.MoveTo(destination, onArrived);
    }

    private void HandleCarrierDelivered(Resource resource)
    {
        ResourceDelivered?.Invoke(this, resource);
    }
}