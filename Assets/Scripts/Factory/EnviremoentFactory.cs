using UnityEngine;

public class EnviromentFactory : BaseFactory<EnviromentDB,EnviromentType>
{
    
}

public enum EnviromentType
{
    Drum,
    Water,
    Mountain
}