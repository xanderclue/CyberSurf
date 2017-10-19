using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class reticle : MonoBehaviour
{
    //image object for the selection radial(required to function for radial bar)
    [SerializeField] Image selectionRadial;

    [SerializeField] Image reticleCenter;

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 1)
        {
            if (selectionRadial.IsActive())
            {
                selectionRadial.gameObject.SetActive(false);
                reticleCenter.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!selectionRadial.IsActive())
            {
                selectionRadial.gameObject.SetActive(true);
                reticleCenter.gameObject.SetActive(true);
            }
        }
    }


    public void UpdateReticleFill(float ratioOfTimePassed)
    {
        selectionRadial.fillAmount = ratioOfTimePassed;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
}