public class EventSelectedObject : SelectedObject
{
    public delegate void SelectedObjectEvent();
    public SelectedObjectEvent OnSelected, OnDeselected, OnSelectSuccess;
    protected override void DeselectedFunction()
    {
        base.DeselectedFunction();
        if (null != OnDeselected)
            OnDeselected();
    }
    protected override void SelectedFunction()
    {
        base.SelectedFunction();
        if (null != OnSelected)
            OnSelected();
    }
    public override void selectSuccessFunction()
    {
        if (null != OnSelectSuccess)
            OnSelectSuccess();
    }
}