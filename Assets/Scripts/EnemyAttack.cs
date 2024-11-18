using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] 
    private GameObject enemyBulletPrefab;

    public void EnemyShoots(Vector2 shootPosition)
    {

        Instantiate(enemyBulletPrefab, shootPosition, Quaternion.Euler(0, 0, -90));

    }

}
