using UnityEngine;

public abstract class BaseFactory<T, U> : MonoBehaviour
    where T : ScriptableObject, IBaseDB
    where U : System.Enum
{
    [SerializeField]
    protected T database;

    public GameObject Build(U type, Vector3 position, Quaternion rotation)
    {
        var data = database.GetByType(type);
        var prefab = data.Item1;
        var offset = data.Item2;
        if (prefab == null)
        {
            Debug.LogWarning("Invalid type provided");
            return null;
        }
        var pos = new Vector3(position.x, offset, position.z);
        return Instantiate(prefab, pos, rotation);
    }

    public virtual (GameObject, float) BuildRandom(Vector3 position, Quaternion rotation)
    {
        Debug.LogWarning("Random build not supported for this factory.");
        return (null,0);
    }
}

