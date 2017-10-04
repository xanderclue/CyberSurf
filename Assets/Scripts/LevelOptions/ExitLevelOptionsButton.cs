using UnityEngine;

public class ExitLevelOptionsButton : SelectedObject
{
    [SerializeField]
    private LevelMenuStuff levelMenuScript = null;
    new private void Start()
    {
        base.Start();
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
    }
    public override void selectSuccessFunction()
    {
        levelMenuScript.ExitMenu();
    }
}