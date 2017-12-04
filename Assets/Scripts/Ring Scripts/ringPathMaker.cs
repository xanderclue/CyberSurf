using System.Collections.Generic;
using UnityEngine;
public class ringPathMaker : MonoBehaviour
{
    [SerializeField] private Stack<Vector3> controlPointsStack = new Stack<Vector3>();
    private bool drawLine = true;
    private CatmullRomSplineDrawn pathDrawer = new CatmullRomSplineDrawn();
    private AI_Race_Mode_Script Race_AI;
    private void TogglePath(bool isOn)
    {
        drawLine = isOn;
    }
    private void OnEnable()
    {
        EventManager.OnSetRingPath += TogglePath;
        Race_AI = GameObject.FindObjectOfType<AI_Race_Mode_Script>();
    }
    private void OnDisable()
    {
        EventManager.OnSetRingPath -= TogglePath;
    }
    public void Init(Transform[] array)
    {
        LineRenderer myself = GetComponentInChildren<LineRenderer>();
        
        if (drawLine)
        {
            controlPointsStack.Push(array[0].position);
            controlPointsStack.Push(array[0].position);
            int firstDuplicate = 0, duplicateNum = 0, lastRing = 0;
            bool foundDuplicate = false;
            RingProperties theRing;
            for (int i = 1; i < array.Length; ++i)
            {
                theRing = array[i].GetComponent<RingProperties>();
                if (theRing.DuplicatePosition)
                {
                    if (!foundDuplicate)
                    {
                        firstDuplicate = i;
                        duplicateNum = theRing.positionInOrder;
                    }
                    else if (duplicateNum != theRing.positionInOrder)
                    {
                        controlPointsStack.Push(array[firstDuplicate].position);
                        duplicateNum = theRing.positionInOrder;
                        firstDuplicate = i;
                        lastRing = i;
                    }
                    foundDuplicate = true;
                }
                else
                {
                    if (foundDuplicate)
                    {
                        controlPointsStack.Push(array[firstDuplicate].position);
                        foundDuplicate = false;
                    }
                    controlPointsStack.Push(array[i].position);
                    lastRing = i;
                }
            }
            controlPointsStack.Push(array[lastRing].position);
            Vector3[] finalPoints = pathDrawer.MakePath(controlPointsStack.ToArray());
            Race_AI.Ring_path = finalPoints;
            myself.positionCount = finalPoints.Length;
            myself.SetPositions(finalPoints);
        }
    }
}