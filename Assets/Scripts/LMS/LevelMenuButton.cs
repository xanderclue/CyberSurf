public class LevelMenuButton : SelectedObject
{
    public delegate void ButtonPressEvent();
    public ButtonPressEvent OnButtonPressed;
    public override void selectSuccessFunction()
    {
        if (null != OnButtonPressed)
            OnButtonPressed();
    }
}