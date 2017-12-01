using UnityEngine;
using UnityEditor;
using System.IO;
public class RingMakerLoaderWizard : ScriptableWizard
{
    private RingMakerRecorder.PositionRotation[] rings = null;
    [SerializeField] private GameObject ringPrefab = null;
    int sceneIndex = -1;
    [MenuItem("Cybersurf Tools/Add Rings...")]
    private static void InitRingMakerLoaderWizard()
    {
        DisplayWizard<RingMakerLoaderWizard>("Add Rings", "Create Rings").OnWizardUpdate();
    }
    private void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (File.Exists(Application.persistentDataPath + $"/rings{sceneIndex}.dat"))
        {
            if (null != ringPrefab)
                isValid = true;
        }
        else
            errorString = "File Not Found: " + Application.persistentDataPath + $"/rings{sceneIndex}.dat";
    }
    private void OnWizardCreate()
    {
        FileStream file = File.Open(Application.persistentDataPath + $"/rings{sceneIndex}.dat", FileMode.Open);
        try { rings = (RingMakerRecorder.PositionRotation[])(new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(file)); }
        catch { return; }
        finally { file.Close(); }
        foreach (RingMakerRecorder.PositionRotation ring in rings)
            Instantiate(ringPrefab, ring.position, ring.rotation);
    }
}