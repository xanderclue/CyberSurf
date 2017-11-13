using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class positionRecorder : MonoBehaviour
{
    public Queue<Vector3> positions = new Queue<Vector3>();
    public Queue<Quaternion> rotations = new Queue<Quaternion>();
    private void OnEnable() { SceneManager.sceneLoaded += Clear; }
    private void OnDisable() { SceneManager.sceneLoaded -= Clear; }
    private void Clear(Scene s, LoadSceneMode m)
    {
        positions.Clear();
        rotations.Clear();
    }
    private void FixedUpdate()
    {
        positions.Enqueue(transform.position);
        rotations.Enqueue(transform.rotation);
    }
}