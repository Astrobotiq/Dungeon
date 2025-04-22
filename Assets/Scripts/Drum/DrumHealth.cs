using System.Collections;
using UnityEngine;

public class DrumHealth : IHealth
{
    [SerializeField]
    private GameObject spawner;

    [SerializeField] 
    private float waitInterval = 0.2f;
    
    [SerializeField] 
    private SoundManager soundManager;
    
    public float DrumTakeDamageSoundVolume = 1f;
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(PositionFinder());
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager.PlaySound(SoundType.DrumTakeDamageSound, DrumTakeDamageSoundVolume);

        if (currentHealth <= 0) {
            PlayerManager.Instance.playerListForEnemyAI.Remove(gameObject);
        }
        
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
