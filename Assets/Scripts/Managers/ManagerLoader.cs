using System.Collections.Generic;
using UnityEngine;
public class ManagerLoader : MonoBehaviour
{
    [SerializeField] private GameObject gameManager = null;
    [SerializeField] private List<GameObject> destroyOnLoad = null;
    private static bool isLoaded = false;
    private void Awake()
    {
        Cursor.visible = false;
        if (null != destroyOnLoad)
            foreach (GameObject gObj in destroyOnLoad)
                try { gObj.SetActive(false); Destroy(gObj); } catch { }
        if (!isLoaded)
        {
#if UNITY_EDITOR
            BuildDebugger.InitDebugger();
#endif
            if (null == GameManager.instance)
                Instantiate(gameManager);
            isLoaded = true;
        }
        try { Destroy(gameObject); } catch { }
    }
}