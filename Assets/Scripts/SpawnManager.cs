using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    private IEnumerator _coroutine;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        _coroutine = SpawnRoutine(5f);
        StartCoroutine(_coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Spawn game objects every 5 seconds
    // Create a coroutine of type IEnumerator - Yield events
    // While loop infinite
    IEnumerator SpawnRoutine(float spawnTime)
    {
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
    public void StopSpawning()
    {
        _stopSpawning = true;
    }
}
