using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss Instance { get; private set; } = null;

    private bool bossIsAlive = true;
    private float bossAttackCooldown = 1f;
    private float movimentSpeed = 0.5f;
    private int movementCicle = 0;
    private GameManager gameManager = null;
    private List<GameObject> shootingPoints = new();
    private bool isExploding = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameManager = GameManager.Instance;
        movementCicle = Random.Range(1, 3);
        if (movementCicle == 2)
        {
            movementCicle = -1;
        }
        foreach (Transform child in transform)
        {
            shootingPoints.Add(child.gameObject);
        }
        StartCoroutine(BossAttack());
    }
    // Start is called before the first frame update

    void Update()
    {
        if (!isExploding)
        {
            transform.position += movementCicle * movimentSpeed * Time.deltaTime * Vector3.up;
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Border"))
        {

            movementCicle *= -1;
        }


    }
    private IEnumerator BossAttack()
    {
        
        while (!isExploding)
        {
            yield return new WaitForSeconds(bossAttackCooldown);
            bossIsAlive = gameManager.GetBossStatus();
            if (bossIsAlive)
            {
                int randomNumber = Random.Range(0, (shootingPoints.Count));
                GameObject selectedAttackPoint = shootingPoints[randomNumber];
                Vector2 shootPosition = selectedAttackPoint.transform.position;
                EnemyAttack enemyAttackScript = this.GetComponent<EnemyAttack>();
                enemyAttackScript.EnemyShoots(shootPosition);
            }
          
        }
    }
    public void SetIsExploding()
    {
        isExploding = true;
        tag = "Destroied";
    }

}
