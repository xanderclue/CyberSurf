using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOffandLerpOnObjects : MonoBehaviour
{
    Color portalInvis;
    Color portalVis;

    Color menuInvis;
    Color menuVis;

    Color[] standsInvis;
    Color[] standsVis;

    Renderer portal;
    Renderer menu;
    Renderer[] stands;

    GameObject stds;
    GameObject portals;
    GameObject menus;

    float currentTime;
    float timeToPlay;
    // Use this for initialization
    void Start()
    {
        timeToPlay = 15.0f;

        stds = GameObject.FindGameObjectWithTag("boardStands");
        portals = GameObject.FindGameObjectWithTag("worldPortals");
        menus = GameObject.FindGameObjectWithTag("menuSystem");

        stds.SetActive(false);
        portals.SetActive(false);
        menus.SetActive(false);
        //  portal = GameObject.Find("DemoPortal").GetComponentInChildren<Renderer>();
        //  menu = GameObject.Find("EnterMainMenuButton").GetComponent<Renderer>();
        //  stands = GameObject.Find("BoardStands").GetComponentsInChildren<Renderer>(true);
        //
        //
        //  portalInvis = portalVis;
        //  portalInvis.a = 0;
        //
        //  menuInvis = menuVis;
        //  menuInvis.a = 0;
        //
        //  for(int i = 0; i < stands.Length; i++)
        //  {
        //      standsInvis[i] = standsVis[i];
        //      standsInvis[i].a = 0;
        //      stands[i].material.color = standsInvis[i];
        //  }
        //
        //
        //  portal.gameObject.SetActive(false);
        //
        // 
        //  menu.material.color = menuInvis;

    }

    // Update is called once per frame
    void Update()
    {
        if (keepPlayerStill.tutorialOn == false)
        {
               stds.SetActive(true);
            portals.SetActive(true);
              menus.SetActive(true);
            //  currentTime += Time.deltaTime;
            //  portal.material.color = Color.Lerp(portalInvis, portalVis, currentTime / timeToPlay);
            //  menu.material.color = Color.Lerp(menuInvis, menuVis, currentTime / timeToPlay);
            //  for(int i = 0; i < stands.Length; i++)
            //  {
            //      stands[i].material.color = Color.Lerp(standsInvis[i], standsVis[i], currentTime / timeToPlay);
            //  }
        }
    }
}
