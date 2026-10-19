using System;
using System.Collections.Generic;
using UnityEngine;


public enum EventGame
{
    WhenGameWin,
    WhenGameLost,
    AfterCreateCard,
    CheckPosCard,
    OpenCards,
    CloseCards
}
public class Observer : MonoBehaviour
{
    static Dictionary<EventGame, List<Action<object[]>>> Listeners = new Dictionary<EventGame, List<Action<object[]>>>();

    public static void AddObserver(EventGame name, Action<object[]> callback)
    {
        if (!Listeners.ContainsKey(name)) Listeners.Add(name, new List<Action<object[]>>());
        Listeners[name].Add(callback);
    }

    public static void RemoveListeners(EventGame name, Action<object[]> callback)
    {
        if (!Listeners.ContainsKey(name)) return;
        Listeners[name].Remove(callback);
    }

    public static void Notify(EventGame name, params object[] datas)
    {
        if (!Listeners.ContainsKey(name)) return;
        foreach (var item in Listeners[name])
        {
            try
            {
                item?.Invoke(datas);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on invoke :" + e);
            }
        }
    }
}
