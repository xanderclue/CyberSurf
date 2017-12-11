using UnityEngine;
using TMPro;
public class WorldPortalText : MonoBehaviour
{
    [SerializeField] private WorldPortalProperties properties = null;
    [SerializeField] private TextMeshPro theName = null;
    [SerializeField] private Renderer portalRenderer = null;
    private void OnEnable()
    {
        properties.OnSceneIndexChanged += UpdateText;
        properties.SceneIndex = (int)GameSettings.GetEnum("CurrentLevel", ref LevelManager.savedCurrentLevel) + LevelSelectOptions.LevelBuildOffset;
    }
    private void OnDisable()
    {
        properties.OnSceneIndexChanged -= UpdateText;
        LevelManager.savedCurrentLevel = GameSettings.SetEnum("CurrentLevel", (LevelManager.Level)(properties.SceneIndex - LevelSelectOptions.LevelBuildOffset));
    }
    private void UpdateText()
    {
        string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(properties.SceneIndex);
        theName.SetText(path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1));
        portalRenderer.material.mainTexture = LevelManager.GetLevelPreview((LevelManager.Level)(properties.SceneIndex - LevelSelectOptions.LevelBuildOffset)).texture;
    }
}