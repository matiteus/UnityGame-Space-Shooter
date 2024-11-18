using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    //define of the spawner instance
    public static Spawner Instance { get; private set; } = null;
   
    //variables being modified in the engine
    [SerializeField]
    private GameObject enemyShooter; //enemy small shooter prefab
    [SerializeField]
    private GameObject asteroid; //asteroid prefab
    [SerializeField]
    private GameObject boss; //boss prefab


    //other variables
    private float verticalOffset = 3f;//vertical offset for 3 enemy shooters spawn
    public int turn = 0; //controls turn, if 0 spawns asteroid if 1 spawns enemies
    public int enemiesOnScreen = 0; //current enemies on screen, a new wave will only spawn if last enemy was removed
    private List<GameObject> shootersOnScreen = new(); //list with the shooters currently on screen, used to select which shooter will shoot
    private GameManager gameManager = null; // gamemanager instance
    

    private void Start()
    {
        gameManager = GameManager.Instance;
    }


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
    }


    public void EnemyDespawn(Collider2D other)
    {
        enemiesOnScreen = gameManager.GetEnemiesOnScreen();
        enemiesOnScreen--;
        gameManager.SetEmemiesOnScreen(enemiesOnScreen);
        Destroy(other.gameObject);
        if (enemiesOnScreen == 0)
        {
            gameManager.SpawnEnemies();
        }

    }
    private void SpawnBoss()
    {
        gameManager.SetBossStatus(true);
        Instantiate(
                    boss,
                    new Vector2(transform.position.x, transform.position.y),
                    Quaternion.Euler(0, 0, 0));
    }
    public void SpawnShooter()
    {
        GameObject shooter1 = Instantiate(
                    enemyShooter,
                    new Vector2(transform.position.x, verticalOffset),
                    Quaternion.Euler(0, 0, 90));
        GameObject shooter2 = Instantiate(
            enemyShooter,
            new Vector2(transform.position.x, transform.position.y),
            Quaternion.Euler(0, 0, 90));
        GameObject shooter3 = Instantiate(
            enemyShooter,
            new Vector2(transform.position.x, -verticalOffset),
            Quaternion.Euler(0, 0, 90));
        turn--;
        enemiesOnScreen = 3;
        gameManager.SetEmemiesOnScreen(enemiesOnScreen);
        shootersOnScreen.Add(shooter1);
        shootersOnScreen.Add(shooter2);
        shootersOnScreen.Add(shooter3);
        gameManager.SetShootersList(shootersOnScreen);
    }

    public void SpawnAsteroid()
    {
        int randomNumber = Random.Range(1, 4);
        switch (randomNumber)
        {
            case 1:
                Instantiate(
                    asteroid,
                    new Vector2(transform.position.x, verticalOffset),
                    transform.rotation);
                break;
            case 2:
                Instantiate(
                    asteroid,
                    new Vector2(transform.position.x, transform.position.y),
                    transform.rotation);
                break;
            default:
                Instantiate(
                    asteroid,
                    new Vector2(transform.position.x, -verticalOffset),
                    transform.rotation);
                break;
        }
        turn++;
        enemiesOnScreen = 1;
        gameManager.SetEmemiesOnScreen(enemiesOnScreen);
    }

    public void DespawnShooters()
    {
        shootersOnScreen = gameManager.GetShooterList();
        foreach (GameObject shooter in shootersOnScreen)
        {
            Destroy(shooter);
        }
        shootersOnScreen.Clear();
        gameManager.SetShootersList(shootersOnScreen);
        enemiesOnScreen = 0;
        gameManager.SetEmemiesOnScreen(enemiesOnScreen);
        gameManager.StopEnemyShoots();
        SpawnBoss();
    }
}




