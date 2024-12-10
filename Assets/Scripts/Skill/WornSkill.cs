using UnityEngine;

public class WornSkill
{
    uint ID;
    
    public SkillSO Skill
    {
        get
        {
            if(CustomizationManager.Instance == null)
                return null;
            if(_skill == null && ID != 0)
                _skill = CustomizationManager.Instance.GetSkillById(ID);

            return _skill;
        }
    }
    
    /*public GameObject Object;

    public bool isAvaliable
    {
        get
        {
            if (Object == null)
                return true;
            return false;
        }
    }*/

    private SkillSO _skill;
    
    public WornSkill(uint ID)
    {
        this.ID = ID;
    }
}