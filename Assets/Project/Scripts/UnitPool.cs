using UnityEngine;
using UnityEngine.Pool;

public class UnitPool : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _defaultCapacity = 5;
    [SerializeField] private int _maxSize = 20;

    private ObjectPool<Unit> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Unit>(
            createFunc: () => Instantiate(_unitPrefab),
            actionOnGet: unit => unit.gameObject.SetActive(true),
            actionOnRelease: unit => unit.gameObject.SetActive(false),
            actionOnDestroy: unit => Destroy(unit.gameObject),
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
        );
    }

    public Unit Get() => _pool.Get();

    public void Release(Unit unit) => _pool.Release(unit);
}