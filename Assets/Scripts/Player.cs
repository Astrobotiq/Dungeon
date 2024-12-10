using System.Collections;
using DG.Tweening;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Karakterin hangi gridde bulunduğunu gösteriyor
    [SerializeField] public GameObject Grid;

    public WornSkill SelectedSkill;

    public WornSkills WornSkills;

    public bool HasUsedSkill { get; private set; } = false;

    //range karakterin kaç adım gidebileceğini gösteriyor
    [SerializeField, Range(1, 5)] public int range;

    [SerializeField, Range(0.1f, 5)] public float travelTime;
    
    public bool HasTraveled { get; private set; } = false;
    
    [SerializeField] Ease EaseX;
    [SerializeField] Ease EaseZ;

    [SerializeField] TravelType TravelType;

    //UI
    [SerializeField] GameView gameView;

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

            onPositionChange(Grid);
            
            Travel();
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
            HasUsedSkill = true;
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
            gameView.OpenSkillPanel(this);
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
            gameView.OpenSkillPanel(this);
        }
        gameView.OpenPlayerPanel(this);
        PlayerManager.Instance.SetSelectedPlayerFromOutside(this.gameObject);
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
}

public enum TravelType
{
    NORMAL,
    JUMP
}