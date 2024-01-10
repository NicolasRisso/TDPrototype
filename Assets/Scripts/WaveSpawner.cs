using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform enemyPrefab2;
    public Transform purpleSlime;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 3f;
    private float timeToWaitBetweenSpawns = 0.5f;

    private int waveIndex = 1;

    private bool waveSpawning = false;

    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        if (!waveSpawning) countdown -= Time.deltaTime;
    }

    private IEnumerator SpawnWave()
    {
        waveSpawning = true;
        int bigCounter = 0;
        waveIndex++;
        if (waveIndex % 10 == 0)
        {
            timeToWaitBetweenSpawns /= 2;
            if (timeBetweenWaves != 1) timeBetweenWaves--;
        }
        for (int i = 0; i < waveIndex; i++)
        {
            if (waveIndex > 50)
            {
                SpawnEnemy(purpleSlime);
                waveIndex = 1;
                yield return new WaitForSeconds(300f);
            }
            else if (waveIndex > 25)
            {
                SpawnEnemy(enemyPrefab2);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                if (bigCounter >= 10) {
                    SpawnEnemy(enemyPrefab2);
                    bigCounter = 0;
                }
                else
                {
                    SpawnEnemy(enemyPrefab);
                    bigCounter++;
                }
                yield return new WaitForSeconds(timeToWaitBetweenSpawns);
            }
        }
        waveSpawning = false;
    }

    private void SpawnEnemy(Transform prefab)
    {
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
