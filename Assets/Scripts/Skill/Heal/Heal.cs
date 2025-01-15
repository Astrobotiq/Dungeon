using UnityEngine;

public class Heal : ISkillEffect
{
    [SerializeField]
    private int heal;
    public override void StartMoving(Grid targetGrid)
    {
        ApplyEffect(targetGrid);
    }

    public override void ApplyEffect(Grid targetGrid)
    {
        transform.position = targetGrid.transform.position;

        if (targetGrid.gameObject && targetGrid.GridObject)
        {
            if (targetGrid.GridObject.GetComponent<IHealth>())
            {
                targetGrid.GridObject.GetComponent<IHealth>().Heal(heal);
            }
        }
        
        Destroy(this.gameObject);
    }
}
