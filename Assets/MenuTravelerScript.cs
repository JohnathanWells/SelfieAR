using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuTravelerScript : MonoBehaviour {

    [System.Serializable]
	public class Menu
    {
        public string name;
        public bool status;
        public UnityEvent onOn;
        public UnityEvent onOff;

        public void SetOn()
        {
            if (!status)
            {
                status = true;
                onOn.Invoke();
            }
        }

        public void SetOff()
        {
            if (status)
            {
                status = false;
                onOff.Invoke();
            }
        }
    }

    public List<Menu> menus = new List<Menu>();
    Dictionary<string, Menu> dictMenus = new Dictionary<string, Menu>();
    public string startTurningOn;

    void Awake()
    {
        foreach(Menu m in menus)
        {
            dictMenus.Add(m.name, m);
        }

        OnlySetOn(startTurningOn);
    }

    public void SetOn(string withName)
    {
        if (dictMenus.ContainsKey(withName))
        {
            dictMenus[withName].SetOn();
        }
    }

    public void SetOff(string withName)
    {
        if (dictMenus.ContainsKey(withName))
        {
            dictMenus[withName].SetOff();
        }
    }

    public void OnlySetOn(string withName)
    {
        foreach (Menu m in menus)
        {
            if (m.name != withName)
            {
                //Debug.Log("Setting " + m.name + " off");
                m.SetOff();
            }
            else
            {
                //Debug.Log("Setting " + m.name + " on");
                m.SetOn();
            }
        }
    }
}
