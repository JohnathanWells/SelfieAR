using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleFunctionality : MonoBehaviour {
    [System.Serializable]
    public class ToggleFunction
    {
        public bool status;
        public UnityEvent onOn;
        public UnityEvent onOff;
    }

    public List<ToggleFunction> functions;

    public void SetOn(int n)
    {
        functions[n].onOn.Invoke();
        functions[n].status = true;
    }

    public void SetOff(int n)
    {
        functions[n].onOff.Invoke();
        functions[n].status = false;
    }

    public void Toggle(int n)
    {
        if (functions[n].status)
            SetOff(n);
        else
            SetOn(n);
    }

    public void Execute(int n)
    {
        if (functions[n].status)
        {
            functions[n].onOff.Invoke();
        }
        else
        {
            functions[n].onOn.Invoke();
        }
    }

    public void SetOnWithoutExecuting(int n)
    {
        functions[n].status = true;
    }

    public void SetOffWithoutExecuting(int n)
    {
        functions[n].status = false;
    }
}
