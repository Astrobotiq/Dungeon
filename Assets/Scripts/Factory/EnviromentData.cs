using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnviromentData", menuName = "EnviromentData")]
public class EnviromentData : BaseData
{
    public EnviromentType EnviromentType;
    public override Enum GetTypeEnum() => EnviromentType;
}