using UnityEngine;
using TMPro;

public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;

    private ResourceCounter _resourceCounter;

    private void Awake()
    {
        Base.BaseSelected += HandleBaseSelected;

        if (Base.SelectedBase != null)
        {
            HandleBaseSelected(Base.SelectedBase);
        }
    }

    private void OnDestroy()
    {
        Base.BaseSelected -= HandleBaseSelected;
        Unsubscribe();
    }

    private void HandleBaseSelected(Base nBase)
    {
        Unsubscribe();

        _resourceCounter = nBase.GetComponent<ResourceCounter>();

        if (_resourceCounter != null)
        {
            _resourceCounter.ResourceCountChanged += UpdateText;
            UpdateText(_resourceCounter.CurrentCount);
        }
    }

    private void Unsubscribe()
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