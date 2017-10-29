using UnityEngine;
public class EyeRayCaster : MonoBehaviour
{
    [SerializeField] private bool canSelect = true;
    [SerializeField] private Camera myCam = null;
    public float rayCheckLength = 12.0f;
    [SerializeField] private LayerMask layerMask;
    private GameObject preObj = null, curObj = null;
    private reticle reticleScript = null;
    private RaycastHit hit;
    private void Start()
    {
        reticleScript = GetComponent<reticle>();
    }
    private void Update()
    {
        preObj = curObj;
        if (Physics.Raycast(myCam.transform.position, myCam.transform.TransformDirection(Vector3.forward), out hit, rayCheckLength, layerMask))
        {
            if (canSelect)
            {
                SelectedObject selected = hit.collider.GetComponent<SelectedObject>();
                if (null != selected)
                {
                    curObj = selected.gameObject;
                    selected.Selected(reticleScript);
                }
                else
                {
                    Debug.LogWarning("Missing SelectedObject script on object in the " + SelectedObject.LAYERNAME + " layer. (" + BuildDebugger.GetHierarchyName(hit.collider.gameObject) + ")");
                    curObj = null;
                }
            }
        }
        else
            curObj = null;
        if (preObj != null && preObj != curObj)
            preObj.GetComponent<SelectedObject>().Deselected();
    }
    private void SetSelectionLock(bool locked)
    {
        canSelect = !locked;
        curObj?.GetComponent<SelectedObject>().Deselected();
    }
    private void OnEnable()
    {
        EventManager.OnSelectionLock += SetSelectionLock;
    }
    private void OnDisable()
    {
        EventManager.OnSelectionLock -= SetSelectionLock;
    }
}