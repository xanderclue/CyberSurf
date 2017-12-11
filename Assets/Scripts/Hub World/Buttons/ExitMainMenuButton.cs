using UnityEngine;
public class ExitMainMenuButton : SelectedObject
{
    [SerializeField] private EnterMainMenuButton enterMenuObject = null;
    new private void Start()
    {
        base.Start();
        if (null == enterMenuObject)
            enterMenuObject = FindObjectOfType<EnterMainMenuButton>();
    }
    protected override void SuccessFunction()
    {
        GetComponentInParent<MainMenu>().InvokeOnMenuExit();
        respawnAndDespawnSphere.SphereState = true;
        enterMenuObject.gameObject.SetActive(true);
    }
}