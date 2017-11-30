using UnityEngine;
public class ExitLevelOptionsButton : SelectedObject
{
    [SerializeField] private LevelMenuStuff levelMenuScript = null;
    new private void Start()
    {
        base.Start();
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
    }
    protected override void SuccessFunction()
    {
        levelMenuScript.ExitMenu();
    }
}