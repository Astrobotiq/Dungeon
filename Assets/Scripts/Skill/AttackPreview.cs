using UnityEngine;

public class AttackPreview : MonoBehaviour
{
    [SerializeField] 
    private GameObject RotateMark;
    
    [SerializeField] 
    private GameObject FourSidedPushMark;
    
    [SerializeField] 
    private GameObject FourSidedZPlusMark;
    
    [SerializeField] 
    private GameObject FourSidedZMinusMark;
    
    [SerializeField] 
    private GameObject FourSidedXPlusMark;
    
    [SerializeField] 
    private GameObject FourSidedXMinusMark;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviewRotateable(Vector3 position)
    {
        RotateMark.gameObject.SetActive(true);
        RotateMark.gameObject.transform.position = new Vector3(position.x, 0.5f, position.z);
    }

    public void PreviewFourSidedPushable(Vector3 position)
    {
        FourSidedPushMark.gameObject.SetActive(true);
        FourSidedPushMark.gameObject.transform.position = new Vector3(position.x, 0.5f,position.z);

        for (int i = -1; i <= 1; i++)
        {
            if (i == 0)
                continue;
            var zGrid = GridManager.Instance.getGridFromLocation(new Vector3(position.x, 0, position.z + i));

            if (zGrid != null)
            {
                if (i == -1)
                {
                    FourSidedZMinusMark.gameObject.SetActive(true);
                }
                else
                {
                    FourSidedZPlusMark.gameObject.SetActive(true);
                }
            }
            
            var xGrid = GridManager.Instance.getGridFromLocation(new Vector3(position.x + i, 0, position.z));

            if (xGrid != null)
            {
                if (i == -1)
                {
                    FourSidedXMinusMark.gameObject.SetActive(true);
                }
                else
                {
                    FourSidedXPlusMark.gameObject.SetActive(true);
                }
            }
        }
    }

    public void PreviewOneSidedPushable(Vector3 throwerPos, Vector3 targetPos)
    {
        var direction = (targetPos - throwerPos);
        FourSidedPushMark.SetActive(true);

        if (direction.x > 0)
        {
            FourSidedXPlusMark.SetActive(true);
        }
        
        if (direction.x < 0)
        {
            FourSidedXMinusMark.SetActive(true);
        }
        
        if (direction.z > 0)
        {
            FourSidedZPlusMark.SetActive(true);
        }
        
        if (direction.z < 0)
        {
            FourSidedXMinusMark.SetActive(true);
        }
    }

    public void ClosePreviews()
    {
        RotateMark.gameObject.SetActive(false);
        FourSidedPushMark.gameObject.SetActive(false);
        FourSidedZPlusMark.gameObject.SetActive(false);
        FourSidedZMinusMark.gameObject.SetActive(false);
        FourSidedXPlusMark.gameObject.SetActive(false);
        FourSidedXMinusMark.gameObject.SetActive(false);
    }
}
