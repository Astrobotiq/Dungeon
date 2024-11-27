using System.Collections;
using DG.Tweening;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Karakterin hangi gridde bulunduğunu gösteriyor
    [SerializeField]
    public GameObject Grid;
    
    //range karakterin kaç adım gidebileceğini gösteriyor
    [SerializeField, Range(1,5)] 
    public int range;
    
    [SerializeField, Range(0.1f, 5)]
    public float travelTime;

    [SerializeField] Ease EaseX;
    [SerializeField] Ease EaseZ;

    [SerializeField] TravelType TravelType;
   
    public void SetGrid(GameObject Grid, float offset)
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
        
        
        this.Grid = Grid;
        Travel();
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
        this.Grid = Grid;
        transform.position =
            new Vector3(Grid.transform.position.x, Grid.transform.position.y + offset, Grid.transform.position.z);
    }

    //This function will handle all the necessary thing when we click the player.
    //For example if we will make a sound we will send from here
    //if we open a UI its information will go from here
    public void MakeSelectedPlayerThis()
    {
        PlayerManager.Instance.SetSelectedPlayer(this.gameObject);
        Algorithm searchAlghorithm = new();
        searchAlghorithm.startAlgorithm(Grid.GetComponent<Grid>(), range);
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
        
        transform.DOMoveX(Grid.transform.position.x, travelTime).SetEase(EaseX).OnComplete(() =>
        {
            NormalTravelZ();
        }  );
    }

    void NormalTravelZ()
    {
        var diffZ = Grid.transform.position.z-transform.position.z;
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
        var position = new Vector3(Grid.transform.position.x, transform.position.y,transform.position.z);
        var duration = travelTime * diffX;
        transform.DOJump(position, 1, diffX, duration).SetEase(EaseX).OnComplete(() =>
        {
            JumpTravelZ();
        }  );
    }
    
    void JumpTravelZ()
    {
        var diffZ = (int)Mathf.Abs(Grid.transform.position.z - transform.position.z);
        if (diffZ == 0)
        {
            InputManager.Instance.canTakeInput = true;
            return;
        }
        var position = new Vector3(transform.position.x, transform.position.y,Grid.transform.position.z);
        var duration = travelTime * diffZ;
        transform.DOJump(position, 1, diffZ, duration).SetEase(EaseZ);
        InputManager.Instance.canTakeInput = true;
    }
    

    #endregion

    public void Undo(GameObject grid, float offset)
    {
        Grid = grid.gameObject;
        GridManager.Instance.SetSelectedGrid(Grid);
        Travel();
    }
}

public enum TravelType
{
    NORMAL,
    JUMP
}
