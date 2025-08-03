using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private ResourceCarrier _carrier;

    public event Action<Unit> ResourceDelivered;

    private void Awake()
    {
        _carrier = GetComponent<ResourceCarrier>();
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

    private void HandleCarrierDelivered()
    {
        ResourceDelivered?.Invoke(this);
    }
}