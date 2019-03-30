using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.Events;

public class VideoLocator : MonoBehaviour, ITrackableEventHandler {

    public UnityEvent OnTargetDetect;
    public UnityEvent OnTargetLose;
    private TrackableBehaviour mTrackableBehaviour;


    void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTargetDetect.Invoke();
        }
        else
        {
            OnTargetLose.Invoke();
        }
    }
}
