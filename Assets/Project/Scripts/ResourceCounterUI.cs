using UnityEngine;
using TMPro;

public class ResourceCounterUI : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _countText;

    private void OnEnable()
    {
        _base.OnResourceCountChanged += UpdateText;
    }

    private void OnDisable()
    {
        _base.OnResourceCountChanged -= UpdateText;
    }

    private void UpdateText(int newCount)
    {
        _countText.text = $"Ресурсы: {newCount}";
    }
}