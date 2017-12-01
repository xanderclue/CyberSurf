using UnityEngine;
using UnityEditor;
using System.IO;
public class RingMakerLoaderWizard : ScriptableWizard
{
    private RingMakerRecorder.PositionRotation[] rings = null;
    [SerializeField] private GameObject ringPrefab = null;
    [MenuItem("Cybersurf Tools/Add Rings...")]
    private static void InitRingMakerLoaderWizard()
    {
        DisplayWizard<RingMakerLoaderWizard>("Add Rings", "Create Rings").OnWizardUpdate();
    }
    private void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        if (File.Exists(Application.persistentDataPath +
            $"/rings{UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}.dat"))
        {
            if (null != ringPrefab)
                isValid = true;
        }
        else
            errorString = "File Not Found: " + Application.persistentDataPath +
                $"/rings{UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}.dat";
    }
    private void OnWizardCreate()
    {
        FileStream file = File.Open(Application.persistentDataPath +
            $"/rings{UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}.dat", FileMode.Open);
        try { rings = (RingMakerRecorder.PositionRotation[])(new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(file)); }
        catch { return; }
        finally { file.Close(); }
        foreach (RingMakerRecorder.PositionRotation ring in rings)
            Instantiate(ringPrefab, ring.position, ring.rotation);
    }
}