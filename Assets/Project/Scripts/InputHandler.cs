using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private const KeyCode ScanKey = KeyCode.S;

    public event Action OnScanRequested;

    private void Update()
    {
        if (Input.GetKeyDown(ScanKey))
        {
            OnScanRequested?.Invoke();
        }
    }
}