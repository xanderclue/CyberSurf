using UnityEngine;
public class SwitchTabs : SelectedObject
{
    [SerializeField] private MenuTab menuTab = null;
    protected override void SuccessFunction()
    {
        if (null != menuTab)
            GetComponentInParent<MainMenu>().InvokeOnSwitchTabs(menuTab);
    }
}