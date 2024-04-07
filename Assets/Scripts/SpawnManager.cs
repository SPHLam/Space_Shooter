using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _tripleShotPowerupPrefab;


    [SerializeField]
    private GameObject _enemyContainer;


    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine(5f));
        StartCoroutine(SpawnTripleShotRoutine(Random.Range(10f, 15f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Spawn game objects every 5 seconds
    // Create a coroutine of type IEnumerator - Yield events
    IEnumerator SpawnEnemyRoutine(float spawnTime)
    {
        while (!_stopSpawning) // Continuously spawn enemy
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
    IEnumerator SpawnTripleShotRoutine(float spawnTime)
    {
        // every 10-15 seconds, spawn in a triple-shot powerup
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6, 0);
            Instantiate(_tripleShotPowerupPrefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
    }
    public void StopSpawning()
    {
        _stopSpawning = true;
    }
}
