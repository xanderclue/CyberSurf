public class LevelMenuButton : SelectedObject
{
    public delegate void ButtonPressEvent();
    public ButtonPressEvent OnButtonPressed;
    public override void SuccessFunction()
    {
        if (null != OnButtonPressed)
            OnButtonPressed();
    }
}