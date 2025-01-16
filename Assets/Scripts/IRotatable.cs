using UnityEngine;

public abstract class IRotatable : MonoBehaviour
{
    public abstract void Rotate(Vector3 direction, int rotateDegree);
}
