using UnityEngine;
using TMPro;

public class ResourceCounterUI : MonoBehaviour
{
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private TextMeshProUGUI _countText;

    private void OnEnable()
    {
        _resourceCounter.OnResourceCountChanged += UpdateText;
    }

    private void OnDisable()
    {
        _resourceCounter.OnResourceCountChanged -= UpdateText;
    }

    private void UpdateText(int newCount)
    {
        _countText.text = $"Ресурсы: {newCount}";
    }
}