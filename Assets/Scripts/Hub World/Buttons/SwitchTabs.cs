using UnityEngine;
public class SwitchTabs : SelectedObject
{
    [SerializeField] private MenuTab menuTab = null;
    protected override void SuccessFunction() => GetComponentInParent<MainMenu>().InvokeOnSwitchTabs(menuTab);
}