using UnityEngine;
public class TooltipObject : MonoBehaviour
{
    [SerializeField, LabelOverride("Grab from Parent")]
    private bool m_grabTextFromParent = false;
    [Multiline, SerializeField]
    private string m_tooltipText = "";
    private void Awake()
    {
        if (m_grabTextFromParent)
        {
            TooltipObject[] theParents = GetComponentsInParent<TooltipObject>();
            if (theParents.Length > 1)
                m_tooltipText = theParents[1].m_tooltipText;
            else
            {
                SelectedObject theOtherParent = GetComponentInParent<SelectedObject>();
                if (null != theOtherParent)
                    m_tooltipText = theOtherParent.tooltipText;
            }
        }
        gameObject.layer = LayerMask.NameToLayer(SelectedObject.LAYERNAME);
        Collider theCollider = 0 != GetComponents<Collider>().Length ? gameObject.GetComponent<Collider>() : gameObject.AddComponent<BoxCollider>();
        SelectedObject selectedObject = gameObject.GetComponent<SelectedObject>() ?? gameObject.AddComponent<EventSelectedObject>();
        if (theCollider is MeshCollider)
            (theCollider as MeshCollider).convex = true;
        theCollider.isTrigger = true;
        selectedObject.tooltipText = m_tooltipText;
        selectedObject.tooltipOnly = true;
        selectedObject.SetupTooltipOnly();
        Destroy(this);
    }
}