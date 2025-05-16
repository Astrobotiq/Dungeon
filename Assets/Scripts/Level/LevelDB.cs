using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelDB : Singleton<LevelDB>
{
       [SerializeField] 
       List<LevelSO> Levels;

       [SerializeField] 
       private TutorialLevelSO tutorialLevelSo;

       public TutorialLevelSO GetTutorialLevel() => tutorialLevelSo;

       public List<LevelSO> GetAllLevels() => Levels;

       public LevelSO GetRandomLevel() => Levels.GetRandom();

       public LevelSO GetLevelByIndex(int index)
       {
              if (index>=Levels.Count)
              {
                     return GetRandomLevel();
              }

              return Levels[index];
       }
}
