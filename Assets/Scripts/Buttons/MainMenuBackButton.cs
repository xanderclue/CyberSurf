public class MainMenuBackButton : SelectedObject
{
    public override void SuccessFunction()
    {
        if (null != GetComponentInParent<MainMenu>().OnBackButtonPressed)
            GetComponentInParent<MainMenu>().OnBackButtonPressed();
    }
}