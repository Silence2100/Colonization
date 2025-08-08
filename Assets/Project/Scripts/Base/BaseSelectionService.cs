using System;
using UnityEngine;

public class BaseSelectionService : MonoBehaviour
{
    private Base _selectedBase;

    public Base SelectedBase => _selectedBase;

    public event Action<Base> BaseSelected;

    public void SelectBase(Base baseToSelect)
    {
        if (baseToSelect == _selectedBase)
        {
            return;
        }

        _selectedBase = baseToSelect;

        BaseSelected?.Invoke(_selectedBase);
    }
}