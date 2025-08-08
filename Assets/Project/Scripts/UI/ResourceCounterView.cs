using UnityEngine;
using TMPro;

public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private BaseSelectionService _baseSelectionService;

    private ResourceCounter _resourceCounter;

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
        _baseSelectionService.BaseSelected -= OnBaseSelected;
        UnsubscribeFromCounter();
    }

    private void OnBaseSelected(Base selectedBase)
    {
        UnsubscribeFromCounter();

        _resourceCounter = selectedBase.GetComponent<ResourceCounter>();

        if (_resourceCounter != null)
        {
            _resourceCounter.ResourceCountChanged += UpdateText;
            UpdateText(_resourceCounter.CurrentCount);
        }
    }

    private void UnsubscribeFromCounter()
    {
        if (_resourceCounter != null)
        {
            _resourceCounter.ResourceCountChanged -= UpdateText;
            _resourceCounter = null;
        }
    }

    private void UpdateText(int value)
    {
        _countText.text = $"Ресурсы: {value}";
    }
}