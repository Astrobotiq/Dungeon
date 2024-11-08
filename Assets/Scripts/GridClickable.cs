using UnityEngine;

public class GridClickable : IClickable
{
    public override void onLeftClick()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }
    
    public override void onRightClick()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
    }
}