using Unity.VisualScripting;

public class WornSkills
{
    public WornSkill Passive;

    public WornSkill Normal;

    public WornSkill Special;
    
    public WornSkills() {}

    public WornSkills(WornSkills wornSkills)
    {
        Passive = wornSkills.Passive;

        Normal = wornSkills.Normal;

        Special = wornSkills.Special;
    }

    public void SetWornSkill(WornSkill wornSkill)
    {
        switch (wornSkill.Skill.SkillType)
        {
            case SkillType.Passive:
                Passive = wornSkill;
                break;
            case SkillType.Normal:
                Normal = wornSkill;
                break;
            case SkillType.Special:
                Special = wornSkill;
                break;
        }
    }

    public void TakeOffWornSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.Passive:
                Passive = null;
                break;
            case SkillType.Normal:
                Normal = null;
                break;
            case SkillType.Special:
                Special = null;
                break;
        }
    }

    public WornSkill GetSkill(SkillType type)
    {
        WornSkill skill = null;
        switch (type)
        {
            case SkillType.Passive:
                skill = Passive ;
                break;
            case SkillType.Normal:
                skill = Normal;
                break;
            case SkillType.Special:
                skill = Special;
                break;
        }

        return skill;
    }
}