using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    // Array of waves
    public Wave[] waves;
    public Button waveStartButton;
    public float timeBetweenWave;
    // Spawn point for enemies
    public Transform spawnPoint;
    public Transform[] enemyTransforms;
    private int currentWaveIndex = 0;
    private bool waveInProgress = false;

    // Event to signal the end of a wave
    public event Action WaveEnded;

    private void Start()
    {
        // Subscribe to the WaveEnded event
        WaveEnded += OnWaveEnded;
    }

    private void Update()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            if (AllEnemiesDestroyed())
            {
               // StartCoroutine(StartNextWaveAfterDelay());
            }
        }
        if (currentWaveIndex >= waves.Length && AllEnemiesDestroyed())
        {
            Time.timeScale = 0f;
        }
    }

    public void StartNextWaveWithButton()
    {
        if (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            SpawnWave(currentWave.enemyTypes);
            waveInProgress = true;
            StartCoroutine(EndWaveAfterDuration(currentWave.delayBeforeNextWave));
            StartCoroutine(TimeBetweenToStartNextWave());
            waveStartButton.gameObject.SetActive(false);
        }
    }
    IEnumerator TimeBetweenToStartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWave);
        waveStartButton.gameObject.SetActive(true);
    }
    private IEnumerator StartNextWaveAfterDelay()
    {
        yield return new WaitForSeconds(waves[currentWaveIndex].delayBeforeNextWave);
        StartNextWaveWithButton();
    }

    private IEnumerator EndWaveAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        waveInProgress = false;
        currentWaveIndex++;
        WaveEnded?.Invoke(); // Signal the end of the wave
    }

    public void SpawnWave(EnemyType[] enemyTypes)
    {
        StartCoroutine(SpawnEnemiesWithRate(enemyTypes));
    }

    private IEnumerator SpawnEnemiesWithRate(EnemyType[] enemyTypes)
    {
        foreach (EnemyType enemyType in enemyTypes)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                GameObject spawnedEnemy = Instantiate(enemyType.prefab, spawnPoint.position, Quaternion.identity);
                spawnedEnemy.transform.SetParent(spawnPoint.transform);
                yield return new WaitForSeconds(1f / enemyType.spawnRate); // Adjust spawn rate
            }
        }
    }

    public bool AllEnemiesDestroyed()
    {
        enemyTransforms = spawnPoint.GetComponentsInChildren<Transform>(); // Get all child transforms

        if(enemyTransforms.Length == 1)
        {
            Debug.Log("True");
            return true;
        }

        return false;
    }

    // Method to handle the end of the wave
    private void OnWaveEnded()
    {
        // Handle the end of the wave
    }
}

[System.Serializable]
public class Wave
{
    public float delayBeforeNextWave;
    public EnemyType[] enemyTypes;
}

[System.Serializable]
public class EnemyType
{
    public GameObject prefab;
    public int count;
    public float spawnRate; // Added spawn rate field
}
