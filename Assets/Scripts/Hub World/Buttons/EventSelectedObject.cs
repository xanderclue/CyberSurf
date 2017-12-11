public class EventSelectedObject : SelectedObject
{
    public delegate void SelectedObjectEvent();
    public event SelectedObjectEvent OnSelected, OnDeselected, OnSelectSuccess;
    protected override void DeselectedFunction() => OnDeselected?.Invoke();
    protected override void SelectedFunction() => OnSelected?.Invoke();
    protected override void SuccessFunction() => OnSelectSuccess?.Invoke();
}