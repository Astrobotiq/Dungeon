using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Dungeon/Enemy", order = 3)]
public class EnemySO : BaseData
{
    public EnemyType EnemyType;
    public override Enum GetTypeEnum() => EnemyType;
}