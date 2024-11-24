using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    
    public Wave[] waves;
    public Button waveStartButton;
    public float timeBetweenWave;
   
    public Transform spawnPoint;
    public Transform[] enemyTransforms;
    private int currentWaveIndex = 0;
    private bool waveInProgress = false;

   
    public event Action WaveEnded;

    private void Start()
    {
        
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
        WaveEnded?.Invoke(); 
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
                yield return new WaitForSeconds(1f / enemyType.spawnRate); 
            }
        }
    }

    public bool AllEnemiesDestroyed()
    {
        enemyTransforms = spawnPoint.GetComponentsInChildren<Transform>(); 

        if(enemyTransforms.Length == 1)
        {
            Debug.Log("True");
            return true;
        }

        return false;
    }

   
    private void OnWaveEnded()
    {
       
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
    public float spawnRate; 
}
