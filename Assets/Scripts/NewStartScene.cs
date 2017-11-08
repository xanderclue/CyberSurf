using UnityEngine;
using UnityEngine.SceneManagement;
public class NewStartScene : MonoBehaviour
{
    private void Awake()
    {
        BuildDebugger.InitDebugger();
        SceneManager.LoadScene(1);
    }
}