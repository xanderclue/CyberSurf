using UnityEngine;
//using UnityEditor;

public class EnterMainMenuButton : SelectedObject
{
    [SerializeField]
    private GameObject menuBox = null;
    [SerializeField]
    private Transform lockTransform = null;
    [SerializeField]
    private MainMenu menuSystem = null;
    [SerializeField]
    private Material hoverMat = null;
    [SerializeField]
    private Material noHoverMat = null;
    private MeshRenderer meshRenderer = null;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (null == menuSystem)
            menuSystem = GetComponentInParent<MainMenu>();
        if (null == noHoverMat)
            noHoverMat = meshRenderer.material;
        if (null == hoverMat)
            hoverMat = noHoverMat;
        meshRenderer.material = noHoverMat;
    }
    [SerializeField, Tooltip("degrees per second")]
    private float rotationSpeed = 111.0f;
    private void Update()
    {
        gameObject.transform.Rotate(0.0f, Time.deltaTime * rotationSpeed, 0.0f);
    }

    public override void selectedFuntion()
    {
        base.selectedFuntion();
        meshRenderer.material = hoverMat;
    }
    public override void deSelectedFunction()
    {
        base.deSelectedFunction();
        meshRenderer.material = noHoverMat;
    }
    public override void selectSuccessFunction()
    {
        if (null != menuBox && null != menuSystem)
        {
            menuBox.SetActive(true);
            if (null != lockTransform)
                GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(lockTransform.position, lockTransform.rotation);
            else
                GameManager.player.GetComponent<PlayerMenuController>().LockPlayerToPosition(GameManager.player.transform.position);
            menuSystem.mainTab.EnableButtons();
            gameObject.SetActive(false);
        }
    }
}