using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xander.NullConversion;
public class RingMakerRecorder : MonoBehaviour
{
    [System.Serializable]
    public struct Vector3Serialized
    {
        public float x, y, z;
        private Vector3Serialized(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public static implicit operator Vector3(Vector3Serialized value) => new Vector3(value.x, value.y, value.z);
        public static implicit operator Vector3Serialized(Vector3 value) => new Vector3Serialized(value.x, value.y, value.z);
    }
    [System.Serializable]
    public struct QuaternionSerialized
    {
        public float x, y, z, w;
        private QuaternionSerialized(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public static implicit operator Quaternion(QuaternionSerialized value) => new Quaternion(value.x, value.y, value.z, value.w);
        public static implicit operator QuaternionSerialized(Quaternion value) => new QuaternionSerialized(value.x, value.y, value.z, value.w);
    }
    [System.Serializable]
    public struct PositionRotation
    {
        public Vector3Serialized position;
        public QuaternionSerialized rotation;
        public PositionRotation(Transform transform)
        {
            if (null != transform)
            {
                position = transform.position;
                rotation = transform.rotation;
            }
            else
            {
                position = Vector3.zero;
                rotation = Quaternion.identity;
            }
        }
        public static implicit operator PositionRotation(Transform transform) => new PositionRotation(transform);
    }
    private List<PositionRotation> rings = null;
    [SerializeField] private GameObject placeHolderPrefab = null;
    private int sceneIndex = -1;
    private void Awake()
    {
        rings = new List<PositionRotation>();
        SceneManager.sceneLoaded += SceneLoaded;
        SceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (enabled) SaveRings();
        enabled = GameMode.Free == GameManager.gameMode && scene.buildIndex >= LevelSelectOptions.LevelBuildOffset;
    }
    private void OnDisable() => SaveRings();
    private void OnDestroy() => SceneManager.sceneLoaded -= SceneLoaded;
    private void AddRing()
    {
        PositionRotation temp = GameManager.player.ConvertNull()?.transform;
        rings.Add(temp);
        if (null != placeHolderPrefab)
            Instantiate(placeHolderPrefab, temp.position, temp.rotation);
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void SaveRings()
    {
        if (0 == rings.Count) return;
        PositionRotation[] theRings = rings.ToArray();
        FileStream file = File.Create(Application.persistentDataPath + $"/rings{sceneIndex}.dat");
        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(file, theRings);
        file.Close();
        rings.Clear();
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.R) &&
            (Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift))) ||
            Input.GetKeyDown(KeyInputManager.XBOX_B))
            AddRing();
    }
}