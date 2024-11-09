using UnityEngine;

public class GridClickable : IClickable
{
    public override void onLeftClick()
    {
        GridManager.Instance.SetSelectedGrid(this.gameObject);    }
    
    public override void onRightClick()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }
}