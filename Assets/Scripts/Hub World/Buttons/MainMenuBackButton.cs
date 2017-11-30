public class MainMenuBackButton : SelectedObject
{
    protected override void SuccessFunction()
    {
        GetComponentInParent<MainMenu>().InvokeOnBackButtonPressed();
    }
}