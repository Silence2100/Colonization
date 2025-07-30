using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;
    public bool IsReserved { get; private set; } = false;

    public event Action<Resource> ReturnToPool;

    public void Initialize()
    {
        IsCollected = false;
        IsReserved = false;
        gameObject.SetActive(true);
    }

    public void Reserve()
    {
        if (IsReserved || IsCollected)
        {
            return;
        }

        IsReserved = true;
    }

    public void Unreserve()
    {
        if (IsReserved == false)
        {
            return;
        }

        IsReserved = false;
    }

    public void Collect()
    {
        if (IsCollected)
        {
            return;
        }

        IsCollected = true;
    }

    public void ReturnPool()
    {
        ReturnToPool?.Invoke(this);
    }
}