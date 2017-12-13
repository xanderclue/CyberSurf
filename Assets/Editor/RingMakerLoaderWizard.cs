using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;
public class RingMakerLoaderWizard : ScriptableWizard
{
    public static GameObject theRingPrefab = null;
    private RingMakerRecorder.PositionRotation[] rings = null;
    [SerializeField] private GameObject ringPrefab = null;
    int sceneIndex = -1;
    [MenuItem("Cybersurf Tools/Add Rings from RingMaker")]
    private static void InitRingMakerLoaderWizard()
    {
        if (null == theRingPrefab)
            theRingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Object Prefabs/Ring_10Meters.prefab");
        RingMakerLoaderWizard wizard = DisplayWizard<RingMakerLoaderWizard>("Add Rings", "Create Rings");
        wizard.ringPrefab = theRingPrefab;
        wizard.OnWizardUpdate();
        if (wizard.isValid)
        {
            wizard.OnWizardCreate();
            wizard.Close();
        }
    }
    private void OnWizardUpdate()
    {
        isValid = false;
        errorString = "";
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        Transform theParent = new GameObject("Imported Rings").transform, newRing;
        foreach (RingMakerRecorder.PositionRotation ring in rings)
        {
            newRing = (PrefabUtility.InstantiatePrefab(ringPrefab) as GameObject).transform;
            newRing.position = ring.position;
            newRing.rotation = Quaternion.Euler(0.0f, ((Quaternion)ring.rotation).eulerAngles.y, 0.0f);
            newRing.parent = theParent;
            newRing.name = "Ring";
        }
        theRingPrefab = ringPrefab;
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}