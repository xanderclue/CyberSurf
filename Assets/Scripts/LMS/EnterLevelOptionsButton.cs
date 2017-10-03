using UnityEngine;

public class EnterLevelOptionsButton : SelectedObject
{
    [SerializeField]
    private LevelMenuStuff levelMenuScript = null;
    [SerializeField, Tooltip("degrees per second")]
    private float rotationSpeed = 111.0f;
    new private void Start()
    {
        base.Start();
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
    }
    new private void Update()
    {
        base.Update();
        gameObject.transform.Rotate(0.0f, Time.deltaTime * rotationSpeed, 0.0f);
    }
    public override void selectSuccessFunction()
    {
        levelMenuScript.EnterMenu();
    }
}