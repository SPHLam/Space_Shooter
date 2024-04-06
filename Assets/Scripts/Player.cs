using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    private SpawnManager _spawnManager;

    // Player's lives
    [SerializeField]
    private int _lives = 3;
    void Start()
    {
        // transform = object
        // Take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        // Hit the space key to spawn laser object & checking delay for firing
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        // Vector3.left = new Vector3(1, 0, 0);
        // Time.deltaTime : 1s;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        // Movement wasd
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);

        var x = Mathf.Abs(transform.position.x) >= 12 ? transform.position.x * -1 : transform.position.x;
        var y = Mathf.Abs(transform.position.y) >= 7 ? transform.position.y * -1 : transform.position.y;
        transform.position = new Vector3(x, y, 0);
    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate; // Add cooldown delay 
        Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), Quaternion.identity);
    }

    public void Damage()
    {
       _lives--;
        if (_lives < 1)
        {
            Destroy(this.gameObject);
            // Communicate with Spawn Manager and let them know stop spawning enemies
            if(_spawnManager != null)
            {
                _spawnManager.StopSpawning();
            }
            else
            {
                Debug.LogError("The Spawn Manager is null!");
            }
        }
    }
}
