using UnityEngine;

public class PlayerClickable : IClickable
{
    [SerializeField]
    Player Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onLeftClick()
    {
        PlayerManager.Instance.SetSelectedPlayer(this.gameObject);
        GridManager.Instance.SetSelectedGrid(Player.Grid);
    }

    public override void onRightClick()
    {
        throw new System.NotImplementedException();
    }
}
