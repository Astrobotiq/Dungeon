using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public static event Action onMove;

    public void InvokeOnMove()
    {
        onMove?.Invoke();
    }
}
