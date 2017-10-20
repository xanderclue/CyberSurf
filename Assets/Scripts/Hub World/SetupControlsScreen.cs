using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SetupControlsScreen : MonoBehaviour
{
    private BoardManager boardManager;
    [SerializeField] private Sprite loadingImage;
    [SerializeField] private Sprite[] controlsImages;
    [SerializeField] private Image ImageObject;

    void Start()
    {
        ImageObject.sprite = loadingImage;
        StartCoroutine(WaitForDetection());
    }

    IEnumerator WaitForDetection()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManager.instance.boardScript.gamepadEnabled)
            if (UnityEngine.VR.VRDevice.isPresent)
                ImageObject.sprite = controlsImages[0];
            else
                ImageObject.sprite = controlsImages[2];
        else
            ImageObject.sprite = controlsImages[1];
    }
}