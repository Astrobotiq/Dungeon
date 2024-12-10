using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDB", menuName = "Dungeon/SkillDB", order = 1)]
public class SkillDB : ScriptableObject
{
    List<SkillSO> Skills;

    public SkillSO GetSkillByID(uint ID)
    {
        foreach (var skill in Skills)
        {
            if (skill.ID == ID)
            {
                Debug.Log("ID : " + skill.ID + "\n" +
                          "Skill Name : " + skill.SkillName);
                return skill;
            }
        }
        Debug.Log("ID is out of bound");
        return null;
    }

    public List<SkillSO> GetSkillList() => Skills;

}