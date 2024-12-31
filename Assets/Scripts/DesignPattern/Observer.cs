using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
public class Observer : GenericSingleton<Observer>
{
    public delegate void CallBackObserver(object data);

    Dictionary<string, HashSet<CallBackObserver>> dictObserver = new Dictionary<string, HashSet<CallBackObserver>>();
    // Use this for initialization
    public void AddObserver(string topicName, CallBackObserver callbackObserver)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        listObserver.Add(callbackObserver);
    }

    public void RemoveObserver(string topicName, CallBackObserver callbackObserver)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        if (listObserver.Contains(callbackObserver))
        {
            listObserver.Remove(callbackObserver);
        }
    }

    public void Notify(string topicName, object Data)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        foreach (CallBackObserver observer in listObserver)
        {
            observer(Data);
        }
    }

    public void Notify(string topicName)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        foreach (CallBackObserver observer in listObserver)
        {
            observer(null);
        }
    }

    protected HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
    {
        if (!dictObserver.ContainsKey(topicName))
            dictObserver.Add(topicName, new HashSet<CallBackObserver>());
        return dictObserver[topicName];
    }
}
