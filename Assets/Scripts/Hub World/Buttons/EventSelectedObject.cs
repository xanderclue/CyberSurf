public class EventSelectedObject : SelectedObject
{
    public delegate void SelectedObjectEvent();
    public event SelectedObjectEvent OnSelected, OnDeselected, OnSelectSuccess;
    protected override void DeselectedFunction()
    {
        base.DeselectedFunction();
        OnDeselected?.Invoke();
    }
    protected override void SelectedFunction()
    {
        base.SelectedFunction();
        OnSelected?.Invoke();
    }
    protected override void SuccessFunction()
    {
        OnSelectSuccess?.Invoke();
    }
}