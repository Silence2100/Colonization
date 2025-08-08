using UnityEngine;

public class BaseConstructionService : MonoBehaviour
{
    [SerializeField] private UnitAssignment _unitAssignment;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private int _resourcesNeededForConstruction = 5;

    private BaseFlagPlacement _flagPlacement;
    private Vector3? _constructionTarget;
    private int _resourcesAtFlagPlacement;

    public bool HasConstructionTarget() => _constructionTarget.HasValue;

    private void Awake()
    {
        _flagPlacement = GetComponent<BaseFlagPlacement>();
    }

    public void SetConstructionTarget(Vector3 targetPosition)
    {
        _constructionTarget = targetPosition;
    }

    public void OnResourceDelivered(int currentResourceCount)
    {
        if (_constructionTarget.HasValue == false || currentResourceCount < _resourcesNeededForConstruction || _unitAssignment.FreeUnits.Count == 0)
        {
            return;
        }

        _unitAssignment.SpendResources(_resourcesNeededForConstruction);

        Unit builder = _unitAssignment.FreeUnits[0];
        _unitAssignment.UnregisterUnit(builder);

        Vector3 buildPosition = _constructionTarget.Value;
        builder.MoveToPosition(buildPosition, () => OnBuilderArrived(builder, buildPosition));

        _constructionTarget = null;
    }

    private void OnBuilderArrived(Unit builder, Vector3 buildPosition)
    {
        _flagPlacement.ClearFlag();

        Base nextBase = Instantiate(_basePrefab, buildPosition, Quaternion.Euler(90f, 0f, 0f));

        UnitAssignment assignment = nextBase.GetComponent<UnitAssignment>();
        assignment.RegisterUnit(builder);
    }
}