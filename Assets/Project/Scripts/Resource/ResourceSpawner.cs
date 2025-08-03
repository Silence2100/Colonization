using System;
using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcePool _resourcePool;
    [SerializeField] private Collider _groundCollider;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private float _spawnHeightOffset = 0.1f;

    private Coroutine _spawnCoroutine;
    private Bounds _spawnBounds;

    public event Action<Resource> ResourceSpawned;

    private void Awake()
    {
        _spawnBounds = _groundCollider.bounds;
    }

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Resource resource = _resourcePool.Get();

        float x = UnityEngine.Random.Range(_spawnBounds.min.x, _spawnBounds.max.x);
        float z = UnityEngine.Random.Range(_spawnBounds.min.z, _spawnBounds.max.z);
        float y = _spawnBounds.max.y + _spawnHeightOffset;

        resource.transform.position = new Vector3(x, y, z);
        resource.transform.rotation = Quaternion.identity;

        ResourceSpawned?.Invoke(resource);
    }
}