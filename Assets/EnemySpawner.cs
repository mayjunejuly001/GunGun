using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject meleeEnemyPrefab;   // Melee enemy prefab
    public GameObject rangedEnemyPrefab;  // Ranged enemy prefab
    public int baseMeleeCount = 15;        // Base melee enemies per wave
    public int baseRangedCount = 10;       // Base ranged enemies per wave
    public int maxWaves = 10;             // Max waves before endless mode
    public float spawnDelay = 1f;         // Delay between spawns
    public float waveDelay = 5f;          // Delay before next wave starts
    public Transform playerTransform;
    public Vector2Int spawnRange = new Vector2Int(100, 300);
    public static event System.Action<EnemyAI> onEnemyDie;
    private int currentWave = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (currentWave < maxWaves || maxWaves == 0)  // 0 means infinite waves
        {
            currentWave++;
            int meleeCount = baseMeleeCount + (currentWave * 2);   // More melee over time
            int rangedCount = baseRangedCount + (currentWave / 2); // More ranged but slower increase

            Debug.Log("Wave " + currentWave + " - Spawning " + meleeCount + " melee & " + rangedCount + " ranged enemies.");

            for (int i = 0; i < meleeCount; i++)
            {
                SpawnEnemy(meleeEnemyPrefab);
                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < rangedCount; i++)
            {
                SpawnEnemy(rangedEnemyPrefab);
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitUntil(() => activeEnemies.Count == 0);  // Wait for enemies to die
            yield return new WaitForSeconds(waveDelay);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector3 spawnPos = playerTransform.position +  new Vector3(getRandomSign() * Random.Range(spawnRange.x , spawnRange.y), 0, getRandomSign() * Random.Range(spawnRange.x, spawnRange.y));
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        enemyAI.player = FindFirstObjectByType<PlayerController>();
      
    
        enemyAI.player.onPlayerDies += () => { enemyAI.player = null; };
    
        activeEnemies.Add(enemy);

        // Remove enemy from list when it dies
        enemy.GetComponent<Health>().OnDeath += () => OnEnemyDead(enemyAI);
    }

     private int getRandomSign()
    {
        return Random.Range(0,100) < 50 ? -1 : 1;
    }

    private void OnEnemyDead(EnemyAI enemy)
    {
        activeEnemies.Remove(enemy.gameObject);
        onEnemyDie?.Invoke(enemy);
    }
}
