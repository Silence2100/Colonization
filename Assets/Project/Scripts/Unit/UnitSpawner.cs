using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private float _spawnRadius = 2f;

    public Unit SpawnOne()
    {
        Unit newUnit = Instantiate(_unitPrefab);

        Vector2 circle = Random.insideUnitCircle * _spawnRadius;

        newUnit.transform.position = transform.position + new Vector3(circle.x, 0.1f, circle.y);
        newUnit.transform.rotation = Quaternion.identity;

        return newUnit;
    }
}