using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DrumHealth : IHealth
{
    [SerializeField]
    private GameObject spawner;

    [SerializeField] 
    private float waitInterval = 0.2f;
    
    [SerializeField] 
    private SoundManager soundManager;
    
    public float DrumTakeDamageSoundVolume = 1f;
    
    [SerializeField] 
    private GameObject drumPopupHealthCanvas;
    
    public override void TakeDamage(int damage, bool willPush)
    {
        StartCoroutine(PositionFinder());
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.PlaySound(SoundType.DrumTakeDamageSound, DrumTakeDamageSoundVolume);

        if (currentHealth - damage <= 0) {
            PlayerManager.Instance.playerListForEnemyAI.Remove(gameObject);
            EventManager.Instance.InvokeOnDrumTakeDamage();
        }
        
        base.TakeDamage(damage);
        
        if(drumPopupHealthCanvas==null)
        {
            Debug.Log("Drum Popup Health Canvas assign edilmemiştir");
            return;
        }
        
        Slider slider = drumPopupHealthCanvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        slider.value = GetComponent<DrumHealth>().getHealthPercentage();
        
        IEnumerator PositionFinder()
        {
            var gridList = GridManager.Instance.GridList;
            bool hasSpawned = false;
            GameObject Spawner = null;
            while (!hasSpawned)
            {
                var grid = gridList.GetRandom().GetRandom();

                if (grid.GetComponent<Grid>().GridObject != null)
                {
                    continue;
                }

                Spawner = Instantiate(spawner,
                    new Vector3(grid.transform.position.x, 0.5f, grid.transform.position.z), Quaternion.identity);

                hasSpawned = true;
                yield return new WaitForSeconds(waitInterval);
            }
            EnemyManager.Instance.AddSpawner(Spawner);
        }
    }
}
