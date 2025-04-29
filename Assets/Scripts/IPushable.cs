using System;
using UnityEngine;

public abstract class IPushable : MonoBehaviour
{
    [SerializeField]
    protected float duration = 1f;

    public float GetDuration() => duration; 
    public abstract void Push(Vector3 position);
}