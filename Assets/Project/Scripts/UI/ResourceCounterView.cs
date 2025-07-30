using UnityEngine;
using TMPro;

public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private TextMeshProUGUI _countText;

    private void OnEnable()
    {
        _resourceCounter.ResourceCountChanged += UpdateText;
    }

    private void OnDisable()
    {
        _resourceCounter.ResourceCountChanged -= UpdateText;
    }

    private void UpdateText(int newCount)
    {
        _countText.text = $"Ресурсы: {newCount}";
    }
}