using UnityEngine;

public class UnitSpawnService : MonoBehaviour
{
    [SerializeField] private UnitAssignment _unitAssignment;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private BaseConstructionService _baseConstructionService;
    [SerializeField] private int _resourcesNeededForSpawn = 3;

    public void SpawnInitialUnits(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnAndRegisterUnit();
        }
    }

    public void OnResourceDelivered(int currentResourceCount)
    {
        if (_baseConstructionService.HasConstructionTarget())
        {
            return;
        }

        if (currentResourceCount >= _resourcesNeededForSpawn)
        {
            _unitAssignment.SpendResources(_resourcesNeededForSpawn);
            SpawnAndRegisterUnit();
        }
    }

    public void SpawnAndRegisterUnit()
    {
        Unit unit = _unitSpawner.Spawn();
        _unitAssignment.RegisterUnit(unit);
    }
}