using System;
using UnityEngine;

public class Water : MonoBehaviour
{
    private EnemyBrain _enemyBrain;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyBrain>(out var enemyBrain) && _enemyBrain != enemyBrain)
        {
            Debug.Log("Enemy girdi");
            _enemyBrain = enemyBrain;
            TutorialManager.Instance.EnqueueTutorial(TutorialType.Water);
            enemyBrain.OnDeath();
        }

        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            Debug.Log("Player geldi suyuma girdi.");
            player.IsInWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            player.IsInWater = false;
        }
    }
}
