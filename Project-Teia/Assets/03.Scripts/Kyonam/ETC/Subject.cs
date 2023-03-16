using UnityEngine;
using System.Collections.Generic;

public enum EVENT {
    GOBATTLE,
    ENDBATTLE
}


public class Subject : MonoBehaviour {

    private List<IObserver> observerList = new List<IObserver>();

    public void AttachObserver(IObserver _observer)
    {
        observerList.Add(_observer);
    }

    public void DetachObserver(IObserver _observer)
    {
        observerList.Remove(_observer);
    }

    public void Notify(EVENT _event)
    {
        for(int i = 0; i < observerList.Count; i++)
        {
            observerList[i].OnNotify(_event);
        }
    }
}
