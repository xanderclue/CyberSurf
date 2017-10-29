using UnityEngine;
public class turnOffandLerpOnObjects : MonoBehaviour
{
    [SerializeField, LabelOverride("Objects")] private GameObject[] objs = null;
    private void Start()
    {
        if (keepPlayerStill.tutorialOn)
            foreach (GameObject obj in objs)
                obj.SetActive(false);
        else Destroy(this);
    }
    private void Update()
    {
        if (!keepPlayerStill.tutorialOn)
        {
            foreach (GameObject obj in objs)
                obj.SetActive(true);
            Destroy(this);
        }
    }
}