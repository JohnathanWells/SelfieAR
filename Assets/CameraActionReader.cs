using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraActionReader : MonoBehaviour {

    public float holdingTime = 3f;
    public float errorMarginTime = 0.25f;
    public UnityEvent onTimeCompleted;
    public UnityEvent onOverTime;
    public UnityEvent onStartDetect;
    public UnityEvent onTimeFailed;
    float currentTime = 0f;
    float currentError = 0f;
    bool detecting = false;
    bool alreadyCompleted = false;
	
	// Update is called once per frame
	void Update () {
        if (OpenCVDetection.NormalizedFacePositions.Count > 0)
        {
            if (!detecting)
            {
                alreadyCompleted = false;
                currentTime = 0f;
                currentError = 0f;
                onStartDetect.Invoke();
            }

            currentTime += Time.deltaTime;
            detecting = true;

            if (currentTime >= holdingTime)
            {
                if (!alreadyCompleted)
                    onTimeCompleted.Invoke();

                onOverTime.Invoke();
                alreadyCompleted = true;
            }
        }
        else
        {
            if (detecting && currentError > errorMarginTime)
            {
                onTimeFailed.Invoke();
                detecting = false;
            }

            currentError += Time.deltaTime;
        }
        //Debug.Log(currentTime + "\n" + currentError);
    }

    public void ResetTimer()
    {
        currentTime = 0f;
    }

    public void DisplayMessage(string msg)
    {
        Debug.Log(msg);
    }
}
