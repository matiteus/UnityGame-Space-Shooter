using UnityEngine;
public class PlayerBulletScript : MonoBehaviour
{

    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager gameManager = GameManager.Instance;

        if (other.CompareTag("Asteroid") || other.CompareTag("Shooter"))
        {
            EnemyControl enemyControlScript = other.GetComponent<EnemyControl>();
            enemyControlScript.SetIsExploding();
            Destroy(this.gameObject);
            gameManager.EnemyIsShot(other);
        }
        else if (other.CompareTag("Enemy Bullet"))
        {
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            gameManager.ReduceBossHealth();
        } 
        

    }
}
    