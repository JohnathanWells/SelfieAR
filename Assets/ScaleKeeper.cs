using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleKeeper : MonoBehaviour {

    public Transform parentVideo;
    public Transform chestPivot;
    public Transform feetPivot;
    public float minScale = 1;
    public float maxScale = 15;
    float currentScaleIndex = 0;
    float currentPositionIndex = 1;

    void Awake()
    {
        ScaleVideo(currentScaleIndex);
    }

    public Vector3 getChestPivotVector()
    {
        //return parentVideo.parent.TransformPoint(Vector3.zero) + (parentVideo.position - chestPivot.position);
        return parentVideo.parent.position + (- parentVideo.position + chestPivot.position);
    }

    public Vector3 getFeetPivotVector()
    {
        //return parentVideo.parent.TransformPoint(Vector3.zero) + (parentVideo.position - chestPivot.position);
        //Debug.Log(parentVideo.parent.position + " + " + parentVideo.position + " -- " + chestPivot.position);
        return parentVideo.parent.position - (- parentVideo.position + feetPivot.position);
    }

    public void MoveVideoToPoint(float lerp)
    {
        parentVideo.position = Vector3.Lerp(getFeetPivotVector(), getChestPivotVector(), lerp);
        currentPositionIndex = lerp;
    }

    public void ScaleVideo(float to)
    {
        parentVideo.localScale = Vector3.one * Mathf.LerpUnclamped(minScale, maxScale, to);
        currentScaleIndex = to;
        MoveVideoToPoint(currentPositionIndex);
    }
}
