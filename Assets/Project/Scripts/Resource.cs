using UnityEngine;

public class Resource : MonoBehaviour
{
    private ResourcePool _owningPool;

    public bool IsCollected { get; private set; } = false;
    public bool IsReserved { get; private set; } = false;

    public void Initialize(ResourcePool pool)
    {
        _owningPool = pool;
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
}