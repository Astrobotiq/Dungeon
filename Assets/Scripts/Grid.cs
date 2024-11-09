using UnityEngine;

public class Grid : MonoBehaviour
{
    void OnEnable()
    {
        GridManager.Instance.AddGrid(transform.position, this.gameObject);
    }
    
    
}