using UnityEngine;

public abstract class FactoryManager : Singleton<FactoryManager>
{
    public abstract GameObject Build(string name,Vector3 position, Quaternion quaternion);

    public abstract GameObject BuildRandom(Vector3 position, Quaternion quaternion);
}