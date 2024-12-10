using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Dungeon/Skill", order = 1)]
public class SkillSO : ScriptableObject
{
    //Skill's positive ID.
    public uint ID;

    public string SkillName;
    
    public SkillType SkillType;

    public SkillDamageType SkillDamageType;

    public SearchType SearchType;
    
    public ClassSpeciality ClassSpeciality;

    public int UseLimit = -1;
    
    public Sprite GUISprite;

    public GameObject SkillGO;
    
    public GameObject PlayerEffect;
}