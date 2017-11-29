using UnityEngine;
using TMPro;
public class WorldPortalText : MonoBehaviour
{
    [SerializeField] private WorldPortalProperties properties = null;
    [SerializeField] private TextMeshPro theName = null;
    [SerializeField] private Texture[] portalViews = null;
    [SerializeField] private Renderer portalRenderer = null;
    private void OnEnable()
    {
        properties.OnSceneIndexChanged += UpdateText;
        UpdateText();
    }
    private void OnDisable()
    {
        properties.OnSceneIndexChanged -= UpdateText;
    }
    private void UpdateText()
    {
        string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(properties.SceneIndex);
        theName.SetText(path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1));
        portalRenderer.material.mainTexture = portalViews[properties.SceneIndex - LevelSelectOptions.LevelBuildOffset];
    }
}