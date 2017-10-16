using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPortalScript : MonoBehaviour
{
    GameObject theFadeObj;

    System.Type boxCollider;
    PlayerMenuController pmc;

    private void Start()
    {
        boxCollider = typeof(UnityEngine.CapsuleCollider);
        pmc = GameManager.player.GetComponent<PlayerMenuController>();
        theFadeObj = GameManager.player.GetComponentInChildren<counterRotater>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == boxCollider && other.gameObject.tag == "Board")
        {
            pmc.ToggleMenuMovement(true);

            StartCoroutine(ExitGameCoroutine());
        }
    }

    IEnumerator ExitGameCoroutine()
    {
        float timeIntoFade = 0f;
        float fadeTime = 0.8f;
        float alpha = 0f;

        while (timeIntoFade < fadeTime)
        {
            timeIntoFade += Time.deltaTime;

            alpha = timeIntoFade / fadeTime;
            alpha = Mathf.Clamp01(alpha);

            theFadeObj.GetComponent<Renderer>().material.SetFloat("_AlphaValue", alpha);

            yield return null;
        }

        SaveLoader.save();
        Application.Quit();

        //in case we're in the editor
        if (Application.isEditor)
        {
            yield return new WaitForSeconds(1f);
            pmc.ToggleMenuMovement(false);
            theFadeObj.GetComponent<Renderer>().material.SetFloat("_AlphaValue", 0f);
        }
    }

}
