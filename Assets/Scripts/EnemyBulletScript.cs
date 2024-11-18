using UnityEngine;

public class EnemyBulletScript : MonoBehaviour

{
    GameManager gameManager = GameManager.Instance;

    public float speed = 3f;
    private float enemySpeed;

    // Start is called before the first frame update
    void Start()
    {
        
        enemySpeed = gameManager.GetSpeed();
        speed += enemySpeed;
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

    }
  

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        
        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        { 
            Destroy(this.gameObject);
            other.tag = "Destroied";
            gameManager.GameOverScreen(other.gameObject);

        }
        else if (other.CompareTag("Player Bullet"))
        {
            Destroy(this.gameObject);
        }


    }
}
