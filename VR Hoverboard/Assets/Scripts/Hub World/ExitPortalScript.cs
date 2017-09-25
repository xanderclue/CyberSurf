using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPortalScript : MonoBehaviour
{
    Image theFadeObj;

    System.Type boxCollider;
    PlayerMenuController pmc;

    private void Start()
    {
        boxCollider = typeof(UnityEngine.BoxCollider);
        pmc = GameManager.player.GetComponent<PlayerMenuController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == boxCollider && other.gameObject.tag == "Board")
        {
            pmc.ToggleMenuMovement(true);

            theFadeObj = GameObject.FindGameObjectWithTag("FadeCover").GetComponent<Image>();
            StartCoroutine(ExitGameCoroutine());
        }
    }

    IEnumerator ExitGameCoroutine()
    {
        float timeIntoFade = 0f;
        float fadeTime = 3f;
        float alpha = theFadeObj.color.a;

        while (timeIntoFade < fadeTime)
        {
            timeIntoFade += Time.deltaTime;

            alpha += timeIntoFade / fadeTime;
            alpha = Mathf.Clamp01(alpha);

            theFadeObj.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        //unlock movement in case we're in the editor
        pmc.ToggleMenuMovement(false);
        SaveLoader.save();
        Application.Quit();
        //TODO:: once we fix the fade cover alpha constantly being set in our scene fade scripts, remove the next line
        theFadeObj.color = new Color(0f, 0f, 0f, alpha);
    }
}
