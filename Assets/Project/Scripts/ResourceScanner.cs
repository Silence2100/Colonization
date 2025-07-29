using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 10f;
    [SerializeField] private LayerMask _resourceLayerMask;
    [SerializeField] private int _maxResults = 50;

    private Collider[] _resultsBuffer;

    private void Awake()
    {
        _resultsBuffer = new Collider[_maxResults];
    }

    public List<Resource> Scan(Vector3 position)
    {
        List<Resource> found = new List<Resource>();

        int hitCount = Physics.OverlapSphereNonAlloc(
            position,
            _scanRadius,
            _resultsBuffer,
            _resourceLayerMask
        );

        for (int i = 0; i < hitCount; i++)
        {
            var resourceComp = _resultsBuffer[i].GetComponent<Resource>();

            if (resourceComp != null && resourceComp.IsCollected == false && resourceComp.IsReserved == false)
            {
                found.Add(resourceComp);
            }
        }

        return found;
    }
}