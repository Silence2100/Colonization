using UnityEngine;

public class InitialUnitSpawner : MonoBehaviour
{
    [SerializeField] private int _initialUnitCount = 3;

    private UnitSpawnService _spawnService;

    private void Awake()
    {
        _spawnService = GetComponent<UnitSpawnService>();
    }

    private void Start()
    {
        _spawnService.SpawnInitialUnits(_initialUnitCount);
        Destroy(this);
    }
}