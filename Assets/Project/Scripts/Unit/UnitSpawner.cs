using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private float _spawnRadius = 2f;

    public Unit Spawn()
    {
        Unit unit = Instantiate(_unitPrefab);

        Vector2 circle = Random.insideUnitCircle * _spawnRadius;

        unit.transform.position = transform.position + new Vector3(circle.x, 0.1f, circle.y);
        unit.transform.rotation = Quaternion.identity;

        return unit;
    }
}