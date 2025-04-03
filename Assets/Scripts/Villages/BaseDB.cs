using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDB<T> : ScriptableObject, IBaseDB where T : BaseData
{
    [SerializeField]
    protected List<T> dataList;

    public (GameObject, float) GetByType(System.Enum type)
    {
        foreach (var data in dataList)
        {
            if (data.GetTypeEnum().Equals(type))
            {
                return (data.Prefab, data.Offset);
            }
        }
        return (null, 0f);
    }

    public (GameObject, float) GetRandom()
    {
        if (dataList.Count == 0) return (null, 0f);
        var randomData = dataList[Random.Range(0, dataList.Count)];
        return (randomData.Prefab, randomData.Offset);
    }
}

public abstract class BaseData: ScriptableObject
{
    public abstract System.Enum GetTypeEnum();
    public GameObject Prefab;
    public float Offset;
}

public interface IBaseDB
{
    (GameObject, float) GetByType(System.Enum type);
    (GameObject, float) GetRandom();
}
