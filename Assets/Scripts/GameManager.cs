using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //define of the game manager instance
    public static GameManager Instance {  get; private set; } = null;

    //variables being modified in the engine
    [SerializeField]
    private TextMeshProUGUI textScore; //gameobject for points
    [SerializeField]
    private TextMeshProUGUI endText; //gameobject for victory or gameover
    [SerializeField]
    private float enemyMovimentSpeed = 5f;//value for speed of enemies who move towards the player
    [SerializeField]
    private int bossSpawnPointsTreshold = 10;//value for required points to spawn the boss
    [SerializeField]
    private float enemyAttackCooldown = 2f; //cooldown for enemy attacks
    [SerializeField]
    private int bossHP = 10;

    //definition of the spawner class to be called
    private Spawner spawner;
    private Boss boss;

    //other variables
    public int gamescore = 0; //points
    private float delayForFirstSpawn = 2f;//time for the delay of the first wave of enemies
    public int enemiesOnScreen = 0; //current enemies on screen, a new wave will only spawn if last enemy was removed
    private List<GameObject> shootersOnScreen = new(); //list with the shooters currently on screen, used to select which shooter will shoot
    private int turn = 0; //controls turn, if 0 spawns asteroid if 1 spawns enemies
    private bool bossSpawned = false;


    //end other variables

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    //Spawn related methods
    private void Start()
    {
        //starts the first spawn
        spawner = Spawner.Instance;
        StartCoroutine(SpawnWithDelay());
        StartCoroutine(EnemyAttack());
    }

    //a delay to give the player some time to get used to the scene
    private IEnumerator SpawnWithDelay()
    { 
        yield return new WaitForSeconds(delayForFirstSpawn);
        SpawnEnemies();
    }
    public void SpawnEnemies()

    {
        if (enemiesOnScreen == 0)
        {
            if (!bossSpawned)
            {
                shootersOnScreen.Clear();
                gamescore = GetGamescore();

                if (turn == 1)
                {
                    spawner.SpawnShooter();
                    turn--;
                }
                else
                {
                    spawner.SpawnAsteroid();
                    turn++;
                }
            }
        }
        


    }
    //end spawn related methods
    public void AddScore()
    {
        gamescore++;
        UpdateScore();
        if (gamescore == bossSpawnPointsTreshold)
        {
            spawner.DespawnShooters();
            enemyMovimentSpeed = 0f;
        }
    }

    //score and gameover methods

    //enemy attack and damage methods
    public void EnemyIsShot(Collider2D other)
    {
        switch (other.tag)
        {
            case "Shooter":
                shootersOnScreen.Remove(other.gameObject);
                Destroy(other.gameObject);
                enemiesOnScreen--;
                other.tag = "Destroied";
                AddScore();
                SpawnEnemies();
                break;
            case "Asteroid":
                other.tag = "Destroied";
                Animator animator = other.GetComponent<Animator>();
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                float animationLength = stateInfo.length;
                enemiesOnScreen--;
                animator.SetTrigger("Explode");
                StartCoroutine(DestroyAfterAnimation(other.gameObject, animationLength));
                break;

        }
        
        
    }

    public IEnumerator DestroyAfterAnimation(GameObject hitObject, float animationLength)
    {
        yield return new WaitForSeconds(animationLength+1f);
        Debug.Log("other is" + hitObject.gameObject.name);
        Destroy(hitObject);
        SpawnEnemies();
    }
    public IEnumerator EnemyAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyAttackCooldown);
            if (shootersOnScreen.Count > 0)
            {
                int randomNumber = Random.Range(0, (shootersOnScreen.Count));
                GameObject selectedShooter = shootersOnScreen[randomNumber];
                if (selectedShooter != null)
                {
                    EnemyAttack enemyAttackScript = selectedShooter.GetComponent<EnemyAttack>();
                    Vector2 shootPosition = selectedShooter.transform.position;
                    enemyAttackScript.EnemyShoots(shootPosition);
                }
            }

        }
    }
    public void UpdateScore()
    {
        textScore.text = gamescore.ToString();
    }
    public void GameOverScreen(GameObject other)
    {
        Debug.Log("other is" + other.gameObject.name);
        Animator animatorPlayer = other.GetComponent<Animator>();
        AnimatorStateInfo stateInfoPlayer = animatorPlayer.GetCurrentAnimatorStateInfo(0);
        float animationLengthPlayer = stateInfoPlayer.length;
        animatorPlayer.SetTrigger("Explode");
        StartCoroutine(DestroyAfterAnimation(other, animationLengthPlayer));
        endText.gameObject.SetActive(true);
        endText.text = "GAME OVER";

    }

    //score and gameover methods

    //boss health method
    public void ReduceBossHealth()
    {
        bossHP--;
        if (bossHP == 0)
        {
            boss = Boss.Instance;
            boss.SetIsExploding();
            Animator animator = boss.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length;
            animator.SetTrigger("Explode");
            StartCoroutine(DestroyAfterAnimation(boss.gameObject, animationLength));

        }
    }

    //get and set methods for other classes
    public float GetSpeed()
    {
        return enemyMovimentSpeed;
    }
    public int GetGamescore()
    {
        return gamescore;
    }
    public int GetBossSpawnPoints()
    {
        return bossSpawnPointsTreshold;
    }
    public bool GetBossStatus()
    {
        return bossSpawned;
    }
    public void SetBossStatus(bool isBoss)
    {
        bossSpawned = isBoss;
    }
    public int GetEnemiesOnScreen()
    {
        return enemiesOnScreen;
    }
    public void SetEmemiesOnScreen(int enemies)
    {
        enemiesOnScreen = enemies;
    }
    public List<GameObject> GetShooterList()
    {
        return shootersOnScreen;
    }
    public void SetShootersList(List<GameObject> list)
    {
        shootersOnScreen = list;
    }
    //end get and set methods for other classes

    public void StopEnemyShoots()
    {
        StopCoroutine(EnemyAttack());
    }

}
