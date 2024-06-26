using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerUps;


    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Spawn game objects every 5 seconds
    // Create a coroutine of type IEnumerator - Yield events
    IEnumerator SpawnEnemyRoutine(float spawnTime)
    {
        yield return new WaitForSeconds(2.0f);
        while (!_stopSpawning) // Continuously spawn enemy
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
    // Spawning power up
    IEnumerator SpawnPowerUpRoutine(float spawnTime)
    {
        yield return new WaitForSeconds(2.0f);
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
    }
    public void StopSpawning()
    {
        _stopSpawning = true;
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine(5f));
        StartCoroutine(SpawnPowerUpRoutine(Random.Range(15f, 20f)));
    }
}
