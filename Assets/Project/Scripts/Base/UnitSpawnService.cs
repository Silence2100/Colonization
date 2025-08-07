using UnityEngine;

public class UnitSpawnService : MonoBehaviour
{
    [SerializeField] private UnitAssignment _unitAssignment;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private BaseConstructionService _baseConstructionService;
    [SerializeField] private int _initialUnitCount = 3;
    [SerializeField] private int _resourcesNeededForSpawn = 3;
    [SerializeField] private bool _spawnInitialUnitsOnStart = false;

    private void Start()
    {
        if (_spawnInitialUnitsOnStart)
        {
            for (int i = 0; i < _initialUnitCount; i++)
            {
                SpawnAndRegisterUnit();
            }
        }

        _unitAssignment.ResourceDelivered += HandleResourceDelivered;
    }

    private void OnDestroy()
    {
        _unitAssignment.ResourceDelivered -= HandleResourceDelivered;
    }

    public void DisableInitialSpawn()
    {
        _spawnInitialUnitsOnStart = false;
    }

    private void HandleResourceDelivered(Unit unit, Resource resource)
    {
        if (_baseConstructionService.HasConstructionTarget())
        {
            return;
        }

        if (_unitAssignment.CurrentDeliveredCount() >= _resourcesNeededForSpawn)
        {
            _unitAssignment.SpendResources(_resourcesNeededForSpawn);
            SpawnAndRegisterUnit();
        }
    }

    private void SpawnAndRegisterUnit()
    {
        var unit = _unitSpawner.Spawn();
        _unitAssignment.RegisterUnit(unit);
    }
}