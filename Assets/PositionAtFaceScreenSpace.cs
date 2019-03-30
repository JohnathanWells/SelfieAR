using UnityEngine;
 
public class PositionAtFaceScreenSpace : MonoBehaviour
{
    private float _camDistance;
 
    void Start()
    {
        _camDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
    }
 
    void Update()
    {
        if (OpenCVDetection.NormalizedFacePositions.Count == 0)
            return;
 
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(OpenCVDetection.NormalizedFacePositions[0].x, OpenCVDetection.NormalizedFacePositions[0].y, _camDistance));
    }
}