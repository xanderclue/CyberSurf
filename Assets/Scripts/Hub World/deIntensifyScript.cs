using UnityEngine;
public class deIntensifyScript : MonoBehaviour
{
    private const float timeToMove = 15.0f;
    private Material bluLight = null, betterLight = null;
    private float currentTime = 0.0f;
    private void Start()
    {
        betterLight = GetComponent<Renderer>().material;
        bluLight = Resources.Load<Material>("Materials/lambert1");
    }
    private void Update()
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