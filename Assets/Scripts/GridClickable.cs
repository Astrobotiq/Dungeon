using UnityEngine;

public class GridClickable : IClickable
{
    [SerializeField]
    Grid grid;
    public override void onLeftClick()
    {
        grid.SetSelectedGrid();
    }
    
    public override void onRightClick()
    {
        //this.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }
}