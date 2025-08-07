using UnityEngine;

public class BaseConstructionService : MonoBehaviour
{
    [SerializeField] private UnitAssignment _unitAssignment;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private int _resourcesNeededForConstruction = 5;

    private BaseFlagPlacement _flagPlacement;
    private Vector3? _constructionTarget;
    private int _accumulatedForConstruction = 0;

    public bool HasConstructionTarget() => _constructionTarget.HasValue;

    private void Awake()
    {
        _flagPlacement = GetComponent<BaseFlagPlacement>();
    }

    private void OnEnable()
    {
        _unitAssignment.ResourceDelivered += HandleResourceDelivered;
    }

    private void OnDisable()
    {
        _unitAssignment.ResourceDelivered -= HandleResourceDelivered;
    }

    public void SetConstructionTarget(Vector3 targetPosition)
    {
        _constructionTarget = targetPosition;
        _accumulatedForConstruction = _unitAssignment.CurrentDeliveredCount();
    }

    private void HandleResourceDelivered(Unit unit, Resource resource)
    {
        if (_constructionTarget.HasValue)
        {
            _accumulatedForConstruction++;
            TryStartConstruction();
        }
    }

    private void TryStartConstruction()
    {
        if (_constructionTarget.HasValue == false || _unitAssignment.FreeUnits.Count <= 0 || _accumulatedForConstruction < _resourcesNeededForConstruction)
        {
            return;
        }

        _unitAssignment.SpendResources(_resourcesNeededForConstruction);

        var builder = _unitAssignment.FreeUnits[0];
        _unitAssignment.UnregisterUnit(builder);

        var buildPosition = _constructionTarget.Value;
        builder.GetComponent<UnitMover>().MoveTo(buildPosition, () => OnBuilderArrived(builder, buildPosition));

        _constructionTarget = null;
        _accumulatedForConstruction = 0;
    }

    private void OnBuilderArrived(Unit builder, Vector3 buildPosition)
    {
        _flagPlacement.ClearFlag();

        Base nextBase = Instantiate(_basePrefab, buildPosition, Quaternion.Euler(90f, 0f, 0f));

        UnitSpawnService spawnService = nextBase.GetComponent<UnitSpawnService>();
        spawnService.DisableInitialSpawn();

        UnitAssignment assignment = nextBase.GetComponent<UnitAssignment>();
        assignment.RegisterUnit(builder);
    }
}