using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Village", fileName = "Village")]
public class VillageSO : BaseData
{
    public VillageType VillageType;

    public override Enum GetTypeEnum() => VillageType;
}