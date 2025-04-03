using UnityEngine;

public class EnemyFactory : BaseFactory<EnemyDB, EnemyType>
{
    public override (GameObject,float) BuildRandom(Vector3 position, Quaternion rotation)
    {
        var data = database.GetRandom();
        var prefab = data.Item1;
        var offset = data.Item2;
        if (prefab == null)
        {
            Debug.LogWarning("No enemy found in DB");
            return (null,0);
        }
        var pos = new Vector3(position.x, offset, position.z);
        return (Instantiate(prefab, pos, rotation),offset);
    }
}
