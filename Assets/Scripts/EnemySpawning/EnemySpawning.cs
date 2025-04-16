using UnityEngine;
using System.Collections;

//I used this resource to create the enemy spawning behavior:
//https://discussions.unity.com/t/how-to-make-infinite-wave-spawner-with-different-enemy-types/831059
public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; //All enemy prefabs
    private int enemyCount;
    private int enemiesInWave = 3;
    private int waveNumber = 1;
    public int numberOfWaves = 5;

    public float timeBetweenEnemySpawn = 2;
    public float timeBetweenWaves = 8;

    public Transform[] spawnPoints; //Array of spawn points (just empty game objects around the scene)

    private bool spawningWave;
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {        
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        StartCoroutine(SpawnEnemyWave(enemiesInWave));
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).Length;

        if (!spawningWave && waveNumber <= numberOfWaves && healthBar.currentHealth > 0)
        {
            waveNumber++;
            StartCoroutine(SpawnEnemyWave(enemiesInWave));
            enemiesInWave++;
        }
    }

    IEnumerator SpawnEnemyWave(int enemiesToSpawn)
    {
        spawningWave = true;
        yield return new WaitForSeconds(timeBetweenWaves); //Wait to pause between wave spawning

        for (int i = 0; i < enemiesToSpawn && healthBar.currentHealth > 0; i++)
        {
            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            int spawnChoice = Random.Range(0, spawnPoints.Length);
            Instantiate(randomEnemy, spawnPoints[spawnChoice].position, randomEnemy.transform.rotation);
            yield return new WaitForSeconds(timeBetweenEnemySpawn); //We wait here to give a bit of time between each enemy spawn
        }
        spawningWave = false;
    }
}
