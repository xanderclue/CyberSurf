using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class reticle : MonoBehaviour
{
    [SerializeField] private Image selectionRadial = null, reticleCenter = null;
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (1 != scene.buildIndex)
        {
            if (selectionRadial.IsActive())
            {
                selectionRadial.gameObject.SetActive(false);
                reticleCenter.gameObject.SetActive(false);
            }
        }
        else if (!selectionRadial.IsActive())
        {
            selectionRadial.gameObject.SetActive(true);
            reticleCenter.gameObject.SetActive(true);
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