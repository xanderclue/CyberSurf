using UnityEngine;
public class EnterMainMenuButton : SelectedObject
{
    [SerializeField] private GameObject menuBox = null;
    [SerializeField] private Transform lockTransform = null;
    [SerializeField] private MainMenu menuSystem = null;
    [SerializeField] private Material hoverMat = null, noHoverMat = null;
    [SerializeField, Tooltip("degrees per second")] private float rotationSpeed = 111.0f;
    private MeshRenderer meshRenderer = null;
    new private void Start()
    {
        base.Start();
        meshRenderer = GetComponent<MeshRenderer>();
        if (null == menuSystem)
            menuSystem = GetComponentInParent<MainMenu>();
        if (null == noHoverMat)
            noHoverMat = meshRenderer.material;
        if (null == hoverMat)
            hoverMat = noHoverMat;
        meshRenderer.material = noHoverMat;
    }
    new private void Update()
    {
        base.Update();
        transform.Rotate(0.0f, Time.deltaTime * rotationSpeed, 0.0f);
    }
    protected override void SelectedFunction()
    {
        base.SelectedFunction();
        meshRenderer.material = hoverMat;
    }
    protected override void DeselectedFunction()
    {
        base.DeselectedFunction();
        meshRenderer.material = noHoverMat;
    }
    protected override void SuccessFunction()
    {
        if (null != menuBox && null != menuSystem)
        {
            respawnAndDespawnSphere.SphereState = false;
            menuBox.SetActive(true);
            if (null == lockTransform)
                lockTransform = GameManager.player.transform;
            GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(lockTransform.position, lockTransform.rotation);
            menuSystem.mainTab.EnableButtons();
            gameObject.SetActive(false);
        }
    }
}