using UnityEngine;
using UnityEngine.SceneManagement;
public class NewStartScene : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(1);
    }
}