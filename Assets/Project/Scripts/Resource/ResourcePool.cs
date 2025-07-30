using UnityEngine;
using UnityEngine.Pool;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 50;

    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc:      CreateResourceInstance,
            actionOnGet:     OnGetResource,
            actionOnRelease: OnReleaseResource,
            actionOnDestroy: resource => Destroy(resource.gameObject),
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize:         _maxSize
        );
    }

    private Resource CreateResourceInstance()
    {
        return Instantiate(_resourcePrefab);
    }

    private void OnGetResource(Resource resourceInstance)
    {
        resourceInstance.Initialize();
        resourceInstance.ReturnToPool += HandleResourceReturnRequested;
    }

    private void OnReleaseResource(Resource resourceInstance)
    {
        resourceInstance.ReturnToPool -= HandleResourceReturnRequested;
        resourceInstance.gameObject.SetActive(false);
    }

    private void HandleResourceReturnRequested(Resource resourceInstance)
    {
        _pool.Release(resourceInstance);
    }

    public Resource Get() => _pool.Get();
}