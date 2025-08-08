using UnityEngine;
using TMPro;

public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private BaseSelectionService _baseSelectionService;

    private Base _currentBase;

    private void Awake()
    {
        _baseSelectionService.BaseSelected += OnBaseSelected;

        if (_baseSelectionService.SelectedBase != null)
        {
            OnBaseSelected(_baseSelectionService.SelectedBase);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromCounter();
        _baseSelectionService.BaseSelected -= OnBaseSelected;
    }

    private void OnBaseSelected(Base selectedBase)
    {
        UnsubscribeFromCounter();

        _currentBase = selectedBase;

        if (_currentBase != null)
        {
            _currentBase.ResourceCountChanged += UpdateText;
            UpdateText(_currentBase.CurrentResourceCount);
        }
    }

    private void UnsubscribeFromCounter()
    {
        if (_currentBase != null)
        {
            _currentBase.ResourceCountChanged -= UpdateText;
            _currentBase = null;
        }
    }

    private void UpdateText(int value)
    {
        _countText.text = $"Ресурсы: {value}";
    }
}