using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Karakterin hangi gridde bulunduğunu gösteriyor
    [SerializeField] public GameObject Grid;

    public WornSkill SelectedSkill;
    public GameObject _selectedSkillEffect;
    
    [SerializeField] 
    Transform footPivot;

    [SerializeField] 
    Transform handPivot;

    public WornSkills WornSkills;

    public bool HasUsedSkill { get; private set; } = false;

    //range karakterin kaç adım gidebileceğini gösteriyor
    [SerializeField, Range(1, 5)] public int range;

    [SerializeField, Range(0.1f, 5)] public float travelTime;

    [SerializeField] Move move;
    
    public bool HasTraveled { get; private set; } = false;
    
    [SerializeField] Ease EaseX;
    [SerializeField] Ease EaseZ;

    [SerializeField] TravelType TravelType;

    //UI
    [SerializeField] GameView gameView;

    void Start()
    {
        WornSkills = new WornSkills();
        WornSkill skill = new WornSkill(1);
        WornSkills.SetWornSkill(skill);
    }

    public void SetGrid(GameObject Grid, float offset)
    {
        //Yazılacak skill Check Burada Yapılacak!!!!!
        if (SelectedSkill == null)
        {
            if (this.Grid == Grid)
            {
                return;
            }

            if (!canTravel(Grid))
            {
                PlayerManager.Instance.SetSelectedPlayer(null);
                return;
            }

            //Hareket etmeden önce command oluştulup ekleniyor
            Command command = new Command(this, this.Grid, offset);
            CommandManager.Instance.AddCommand(command);
            
            move.StartMove(this.Grid.GetComponent<Grid>(),Grid.GetComponent<Grid>());
            onPositionChange(Grid);
            //Travel();
            HasTraveled = true;
        }
        else
        {
            //Bu noktada yapılmış olan şeyler:
            // 1-Player'a tıklandı
            // 2-UI'da Player'ın Skill'lerinden bir tanesi seçildi
            // 3-Skill'in search algoritmasından açılan yerlerden bir tanesi seçildi
            // 4-Selected Skill seçili olduğu için buraya geldi.

            //Buradan sonra yapılacak olanlar:
            // 1-Skill'in Throwable'ı instantiate edilecek.
            // 2-Skill'in bitmesi beklenecek (Bu sırada player input veremeyecek.)
            InputManager.Instance.canTakeInput = false;
            
            StartCoroutine(move.Turn(this.Grid.transform.position, Grid.transform.position,(InstantiateSkill)));

            void InstantiateSkill()
            {
                
                var skillGo = SelectedSkill.Skill.SkillGO;
                var go = Instantiate(skillGo, handPivot.position, quaternion.identity);
                go.GetComponent<ISkillEffect>().StartMoving(Grid.GetComponent<Grid>());
                DestroyImmediate(_selectedSkillEffect);
                HasUsedSkill = true;
                InputManager.Instance.canTakeInput = true;
            }
        }
    }

    bool canTravel(GameObject Grid)
    {
        return Mathf.Abs(Grid.transform.position.x - transform.position.x) +
            Mathf.Abs(Grid.transform.position.z - transform.position.z) <= range;
    }

    public void SetGridStart(GameObject Grid, float offset)
    {
        if (this.Grid == Grid)
        {
            return;
        }

        onPositionChange(Grid);
        
        transform.position =
            new Vector3(Grid.transform.position.x, Grid.transform.position.y + offset, Grid.transform.position.z);
    }

    //This function will handle all the necessary thing when we click the player.
    //For example if we will make a sound we will send from here
    //if we open a UI its information will go from here
    public void MakeSelectedPlayerThis()
    {
        if (!HasUsedSkill)
        {
            gameView.OpenSkillPanel(true,this);
        }
        gameView.OpenPlayerPanel(this);
        PlayerManager.Instance.SetSelectedPlayer(this.gameObject);
    }

    public void SetSelectedPlayerFromOutside()
    {
        if (PlayerManager.Instance.GetSelectedPlayer() == this.gameObject)
        {
            return;
        }
        if (!HasUsedSkill)
        {
            gameView.OpenSkillPanel(true,this);
        }
        gameView.OpenPlayerPanel(this);
        PlayerManager.Instance.SetSelectedPlayerFromOutside(this.gameObject);
    }

    public void SetSelectedSkill(SkillType type)
    {
        SelectedSkill = WornSkills.GetSkill(type);
        Debug.Log("Buraya geldik");
        if (SelectedSkill != null && (_selectedSkillEffect == null || SelectedSkill.Skill.PlayerEffect != _selectedSkillEffect))
        {
            _selectedSkillEffect = SelectedSkill.Skill.PlayerEffect;
            _selectedSkillEffect =Instantiate(_selectedSkillEffect, footPivot.position, Quaternion.identity);
            GridManager.Instance.StartSearchForSkill(SelectedSkill.Skill.SearchType);
        }
    }

    #region Travel

    public void Travel()
    {
        switch (TravelType)
        {
            case TravelType.NORMAL:
                NormalTravelX();
                break;
            case TravelType.JUMP:
                JumpTravelX();
                break;
        }
    }

    void NormalTravelX()
    {
        InputManager.Instance.canTakeInput = false;
        var diffX = Grid.transform.position.x - transform.position.x;
        var stepX = diffX / Mathf.Abs(diffX);

        transform.DOMoveX(Grid.transform.position.x, travelTime).SetEase(EaseX).OnComplete(() => { NormalTravelZ(); });
    }

    void NormalTravelZ()
    {
        var diffZ = Grid.transform.position.z - transform.position.z;
        var stepZ = diffZ / Mathf.Abs(diffZ);
        transform.DOMoveZ(Grid.transform.position.z, travelTime).SetEase(EaseZ);
        InputManager.Instance.canTakeInput = true;
    }

    void JumpTravelX()
    {
        InputManager.Instance.canTakeInput = false;
        var diffX = (int)Mathf.Abs(Grid.transform.position.x - transform.position.x);
        if (diffX == 0)
        {
            JumpTravelZ();
            return;
        }

        var position = new Vector3(Grid.transform.position.x, transform.position.y, transform.position.z);
        var duration = travelTime * diffX;
        transform.DOJump(position, 1, diffX, duration).SetEase(EaseX).OnComplete(() => { JumpTravelZ(); });
    }

    void JumpTravelZ()
    {
        var diffZ = (int)Mathf.Abs(Grid.transform.position.z - transform.position.z);
        if (diffZ == 0)
        {
            InputManager.Instance.canTakeInput = true;
            return;
        }

        var position = new Vector3(transform.position.x, transform.position.y, Grid.transform.position.z);
        var duration = travelTime * diffZ;
        transform.DOJump(position, 1, diffZ, duration).SetEase(EaseZ);
        InputManager.Instance.canTakeInput = true;
    }

    #endregion

    public void Undo(GameObject grid, float offset)
    {
        onPositionChange(grid);
        Travel();
        GridManager.Instance.SetSelectedGridFromOutside(Grid.transform.position,false);
        HasTraveled = false;
    }

    public void onDeselected()
    {
        Debug.Log("onDeselect");
        if (_selectedSkillEffect != null)
        {
            Debug.Log("Buradayım");
            DestroyImmediate(_selectedSkillEffect.gameObject);
            _selectedSkillEffect = null;
        }
        
        SelectedSkill = null;
        gameView.OpenSkillPanel(false);
        gameView.OpenPlayerPanel(false);
    }

    private void onPositionChange(GameObject newOne)
    {
        if (Grid != null)
        {
            Grid.GetComponent<Grid>().GridObject = null;
        }
        
        Grid = newOne;
        Grid.GetComponent<Grid>().GridObject = this.gameObject;

    }

    public void SetSelectedSkill(WornSkill wornSkill)
    {
        if (wornSkill == null)
        {
            return;
        }

        SelectedSkill = wornSkill;
        
    }
}