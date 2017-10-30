public class LevelMenuButton : SelectedObject
{
    public delegate void ButtonPressEvent();
    public event ButtonPressEvent OnButtonPressed;
    protected override void SuccessFunction()
    {
        OnButtonPressed?.Invoke();
    }
}