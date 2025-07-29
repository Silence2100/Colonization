using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private ResourceCarrier _carrier;

    public event Action<Unit> OnResourceDelivered;

    private void Awake()
    {
        _carrier = GetComponent<ResourceCarrier>();
        _carrier.OnDelivered += () => OnResourceDelivered?.Invoke(this);
    }

    public void SetTarget(Resource resource, SphereCollider deliveryZone)
    {
        _carrier.CollectAndDeliver(resource, deliveryZone);
    }
}