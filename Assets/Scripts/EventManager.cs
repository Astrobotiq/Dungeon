using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public static event Action<string> onMove;
    public static event Action onPush;
    public static event Action onPlayerTakeDamage;
    public static event Action onEnemyKilled;
    public static event Action onVillageTakeDamage;
    public static event Action onMountainDestroyed;
    public static event Action onSpawnerPrevented;

    public void InvokeOnMove(string name)
    {
        onMove?.Invoke(name);
    }

    public void InvokeOnPush()
    {
        onPush?.Invoke();
    }

    public void InvokeOnPlayerTakeDamage()
    {
        onPlayerTakeDamage?.Invoke();
    }

    public void InvokeOnEnemyKilled()
    {
        onEnemyKilled?.Invoke();
    }
    
    public void InvokeOnVillageTakeDamage()
    {
        onVillageTakeDamage?.Invoke();
    }

    public void InvokeOnMountainDestroyed()
    {
        onMountainDestroyed.Invoke();
    }

    public void InvokeOnSpawnerPrevented()
    {
        onSpawnerPrevented.Invoke();
    }
}
