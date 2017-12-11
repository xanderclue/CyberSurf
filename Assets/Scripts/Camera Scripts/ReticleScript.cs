using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ReticleScript : MonoBehaviour
{
    [SerializeField] private Image selectionRadial = null, reticleCenter = null, background = null;
    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex >= LevelManager.LevelBuildOffset)
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
    private void ChangeReticleColor(Color color)
    {
        color.a = selectionRadial.color.a;
        selectionRadial.color = color;
        color.a = background.color.a;
        background.color = color;
        color.a = reticleCenter.color.a;
        reticleCenter.color = color;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        TextElementControllerScript.OnHudColorChanged += ChangeReticleColor;
        ChangeReticleColor(GameSettings.GetColor("HudColor", selectionRadial.color));
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        TextElementControllerScript.OnHudColorChanged -= ChangeReticleColor;
    }
}