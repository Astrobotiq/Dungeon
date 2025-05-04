using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    
    private GameObject RealSlideBar;
    private GameObject FakeSliderBar;
    private GameObject gridObjectRef;
    
    
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
        ClosePreviewDamageGiven();
    }

    public void PreviewDamageGiven(Vector3 targetPos, int damage)
    {
        if (!GridManager.Instance.getGridFromLocation(targetPos).GridObject) {}
        
        else if (GridManager.Instance.getGridFromLocation(targetPos).GridObject.GetComponent<EnemyHealth>())
        {
            gridObjectRef = GridManager.Instance.getGridFromLocation(targetPos).GridObject;
            RealSlideBar = gridObjectRef.GetComponent<EnemyHealth>().enemyPopupHealthCanvas;
            FakeSliderBar = gridObjectRef.GetComponent<EnemyHealth>().AttackPreviewHealthCanvas;
            
            FakeSliderBar.transform.LookAt(Camera.main.transform.position, Vector3.up);
            
            RealSlideBar.SetActive(false);
            FakeSliderBar.SetActive(true);

            gridObjectRef.GetComponent<EnemyHover>().inAttackPreview = true;
            gridObjectRef.GetComponent<EnemyHover>().enabled = false;

            float EnemyHealthValue = gridObjectRef.GetComponent<EnemyHealth>().getHealth();
            float remainingHealth = EnemyHealthValue - damage;
            
            if (remainingHealth < 0)
            {
                remainingHealth = 0;
            }
            
            float temp_1 = (float) remainingHealth;
            float temp_2 = (float) gridObjectRef.GetComponent<EnemyHealth>().getMaxHealth();
            float temp_3 = Mathf.FloorToInt((temp_1 / temp_2) * 100);
            FakeSliderBar.transform.GetChild(0).gameObject.GetComponent<Slider>().value = temp_3;
            
            StartCoroutine(FakeHealthFlickering());
        }
        
        else if (GridManager.Instance.getGridFromLocation(targetPos).GridObject.GetComponent<VillageHealth>())
        {
            gridObjectRef = GridManager.Instance.getGridFromLocation(targetPos).GridObject;
            RealSlideBar = gridObjectRef.GetComponent<VillageHealth>().villagePopupHealthCanvas;
            FakeSliderBar = gridObjectRef.GetComponent<VillageHealth>().AttackPreviewHealthCanvas;
            
            FakeSliderBar.transform.LookAt(Camera.main.transform.position, Vector3.up);
            
            RealSlideBar.SetActive(false);
            FakeSliderBar.SetActive(true);
            
            gridObjectRef.GetComponent<VillageHover>().inAttackPreview = true;
            gridObjectRef.GetComponent<VillageHover>().enabled = false;

            float VillageHealthValue = gridObjectRef.GetComponent<VillageHealth>().getHealth();
            float remainingHealth = VillageHealthValue - damage;
            
            if (remainingHealth < 0)
            {
                remainingHealth = 0;
            }
            
            float temp_1 = (float) remainingHealth;
            float temp_2 = (float) gridObjectRef.GetComponent<VillageHealth>().getMaxHealth();
            float temp_3 = Mathf.FloorToInt((temp_1 / temp_2) * 100);
            FakeSliderBar.transform.GetChild(0).gameObject.GetComponent<Slider>().value = temp_3;
            
            StartCoroutine(FakeHealthFlickering());
        }
        
        else if (GridManager.Instance.getGridFromLocation(targetPos).GridObject.GetComponent<DrumHealth>())
        {
            gridObjectRef = GridManager.Instance.getGridFromLocation(targetPos).GridObject;
            RealSlideBar = gridObjectRef.GetComponent<DrumHealth>().drumPopupHealthCanvas;
            FakeSliderBar = gridObjectRef.GetComponent<DrumHealth>().AttackPreviewHealthCanvas;
            
            FakeSliderBar.transform.LookAt(Camera.main.transform.position, Vector3.up);
            
            RealSlideBar.SetActive(false);
            FakeSliderBar.SetActive(true);
            
            gridObjectRef.GetComponent<DrumHover>().inAttackPreview = true;
            gridObjectRef.GetComponent<DrumHover>().enabled = false;
            
            float DrumHealthValue = gridObjectRef.GetComponent<DrumHealth>().getHealth();
            float remainingHealth = DrumHealthValue - damage;
            
            if (remainingHealth < 0)
            {
                remainingHealth = 0;
            }
            
            float temp_1 = (float) remainingHealth;
            float temp_2 = (float) gridObjectRef.GetComponent<DrumHealth>().getMaxHealth();
            float temp_3 = Mathf.FloorToInt((temp_1 / temp_2) * 100);
            FakeSliderBar.transform.GetChild(0).gameObject.GetComponent<Slider>().value = temp_3;
            
            StartCoroutine(FakeHealthFlickering());
        }
        
        IEnumerator FakeHealthFlickering() {
            Color initial = FakeSliderBar.transform.GetChild(0).gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
            Color temp = initial;
        
            temp.a = 0.1f;
        
            FakeSliderBar.transform.GetChild(0).gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = temp;
            yield return new WaitForSeconds(0.5f);
        
            FakeSliderBar.transform.GetChild(0).gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = initial;
            yield return new WaitForSeconds(0.5f);
                
            yield return null;
        }
    }
    
    

    public void ClosePreviewDamageGiven()
    {
        if (FakeSliderBar && RealSlideBar)
        {
            if (gridObjectRef.GetComponent<EnemyHover>()) {
                gridObjectRef.GetComponent<EnemyHover>().enabled = true;
                gridObjectRef.GetComponent<EnemyHover>().inAttackPreview = false;
            }
            
            else if (gridObjectRef.GetComponent<VillageHover>()) {
                gridObjectRef.GetComponent<VillageHover>().enabled = true;
                gridObjectRef.GetComponent<VillageHover>().inAttackPreview = false;
            }
            
            else if (gridObjectRef.GetComponent<DrumHover>()) {
                gridObjectRef.GetComponent<DrumHover>().enabled = true;
                gridObjectRef.GetComponent<DrumHover>().inAttackPreview = false;
            }
            
            FakeSliderBar.SetActive(false);
            
            Color color = RealSlideBar.transform.GetChild(0).gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color;
            FakeSliderBar.transform.GetChild(0).gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = color;
        }
    }
}
