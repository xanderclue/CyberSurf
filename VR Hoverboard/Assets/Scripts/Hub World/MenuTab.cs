using System.Collections.Generic;
using UnityEngine;

public class MenuTab : MonoBehaviour
{
    private List<SelectedObject> buttons;
    private void Awake()
    {
        buttons = new List<SelectedObject>();
        SelectedObject[] arr = GetComponentsInChildren<SelectedObject>();
        for (int i = 0; i < arr.Length; ++i)
            buttons.Add(arr[i]);
        if (this != GetComponentInParent<MainMenu>().mainTab)
            DisableButtons();
    }
    public void EnableButtons()
    {
        foreach (SelectedObject button in buttons)
            button.IsDisabled = false;
    }
    public void DisableButtons()
    {
        foreach (SelectedObject button in buttons)
            button.IsDisabled = true;
    }
}