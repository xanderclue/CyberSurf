using System.Collections.Generic;
using UnityEngine;

public class ManagerLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;
    [SerializeField]
    private List<GameObject> destroyOnLoad = null;

    private static bool isLoaded = false;

    //loads our GameManager into the scene,
    //      By doing this instead of having our GameManger already in the scene, we
    //      prevent attached scripts from calling Awake() more than once.
    void Awake()
    {
        if (null != destroyOnLoad)
            foreach (GameObject gObj in destroyOnLoad)
                try { gObj.SetActive(false); Destroy(gObj); } catch { }
        if (!isLoaded)
        {
            BuildDebugger.InitDebugger();
            if (null == GameManager.instance)
                Instantiate(gameManager);
#if UNITY_EDITOR
            try
            {
                const int appid = 735810;
                System.IO.StreamWriter steamAppIdEditor = new System.IO.StreamWriter(Application.dataPath + "/../steam_appid.txt", false, System.Text.Encoding.ASCII);
                steamAppIdEditor.Write(appid);
                steamAppIdEditor.Close();
            }
            catch { }
#endif
            isLoaded = true;
        }
        try { Destroy(gameObject); } catch { }
    }
}