using UnityEngine;

public class Despawn : MonoBehaviour
{
    //used on the walls to despawn everything

    private void OnTriggerEnter2D(Collider2D other)
    {
        Spawner spawner = Spawner.Instance;
        
        if (other.CompareTag("Player Bullet") || other.CompareTag("Enemy Bullet"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            spawner.EnemyDespawn(other);
        }


    }
}
