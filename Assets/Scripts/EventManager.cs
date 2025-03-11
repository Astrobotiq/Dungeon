using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public static event Action<string> onMove;

    public void InvokeOnMove(string name)
    {
        onMove?.Invoke(name);
    }
}
