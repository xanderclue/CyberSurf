using UnityEngine;

public class deIntensifyScript : MonoBehaviour
{
    private const float timeToMove = 15.0f;
    private Material bluLight, betterLight;
    private float currentTime = 0.0f;

    void Start()
    {
        betterLight = gameObject.GetComponent<Renderer>().material;
        bluLight = Resources.Load("lambert1") as Material;
    }

    void Update()
    {
        if (!keepPlayerStill.tutorialOn)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                betterLight.Lerp(betterLight, bluLight, currentTime / timeToMove);
            }
            else
                Destroy(this);
        }
    }
}