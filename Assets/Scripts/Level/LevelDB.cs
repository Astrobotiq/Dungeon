using System.Collections.Generic;
using UnityEngine;

public class LevelDB : Singleton<LevelDB>
{
       [SerializeField] 
       List<LevelSO> Levels;

       public List<LevelSO> GetAllLevels() => Levels;

       public LevelSO GetRandomLevel() => Levels.GetRandom();
}
