using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public static event Action<string> onMove;
    public static event Action onPush;

    public void InvokeOnMove(string name)
    {
        onMove?.Invoke(name);
    }

    public void InvokeOnPush()
    {
        onPush?.Invoke();
    }
}
