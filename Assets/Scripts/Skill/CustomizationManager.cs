using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : Singleton<CustomizationManager>
{
    [SerializeField] 
    SkillDB skillDB;

    public SkillSO GetSkillById(uint ID) => skillDB.GetSkillByID(ID);

    public List<SkillSO> GetSkillList() => skillDB.GetSkillList();

    public SkillSO GetRandomSkill() => skillDB.GetSkillList().GetRandom();
}