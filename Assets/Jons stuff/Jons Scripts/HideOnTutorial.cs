using UnityEngine;
public class HideOnTutorial : MonoBehaviour
{
    [SerializeField, LabelOverride("Objects")] private GameObject[] objs = null;
    private void Start()
    {
        if (keepPlayerStill.tutorialOn)
            foreach (GameObject obj in objs)
                obj.SetActive(false);
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (!keepPlayerStill.tutorialOn)
        {
            foreach (GameObject obj in objs)
                obj.SetActive(true);
            Destroy(gameObject);
        }
    }
}