using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> ReturnToPool;

    public void Initialize()
    {
        gameObject.SetActive(true);
    }

    public void ReturnPool()
    {
        ReturnToPool?.Invoke(this);
    }
}