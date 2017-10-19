﻿using System.Collections.Generic;
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