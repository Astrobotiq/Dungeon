using System;
using UnityEngine;

public class Water : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyBrain>(out var enemyBrain))
        {
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
