using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player speed
    [SerializeField]
    private float _speed = 3.5f;

    // Laser
    [SerializeField]
    private GameObject _laserPrefab;

    // Power up: Triple shot
    [SerializeField]
    private GameObject _tripleShotPrefab;

    // Fire rate
    [SerializeField]
    private float _fireRate = 0.5f;

    // Fire rate delay
    private float _nextFire = 0.0f;

    // Enemy spawn manager
    private SpawnManager _spawnManager;

    // Power up condition
    private bool _isTripleShot = false;
    private bool _isShield = false;

    // Variable reference to the shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;

    // Player's lives
    [SerializeField]
    private int _lives = 3;

    // Score
    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        // transform = object
        // Take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
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
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        var x = Mathf.Abs(transform.position.x) >= 12 ? transform.position.x * -1 : transform.position.x;
        var y = Mathf.Abs(transform.position.y) >= 7 ? transform.position.y * -1 : transform.position.y;
        transform.position = new Vector3(x, y, 0);
    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate; // Add cooldown delay 
        if (!_isTripleShot)
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z), Quaternion.identity);
        } 
        else
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
    }

    // Damaging feature
    public void Damage()
    {
        if (!_isShield)
        {
            _lives--;
            // Updating UI
            _uiManager.UpdateLives(_lives);
            if (_lives < 1)
            {
                Destroy(this.gameObject);

                _spawnManager.StopSpawning();
                _uiManager.ShowGameOver();
            }
        }
        else
        {
            DisableShield();
            return;
        }
    }
    
    // Triple shot powerup
    public void EnableTripleShot()
    {
        _isTripleShot = true;
        StartCoroutine(TripleShotLifespan());
    }
    public void DisableTripleShot()
    {
        _isTripleShot = false;
    }
    // IENumerator TripleShotLifespan
    // wait 5 seconds and disable triple shot
    IEnumerator TripleShotLifespan()
    {
        yield return new WaitForSeconds(5f);
        DisableTripleShot();
    }

    // Speed boost powerup
    public void EnableSpeedBoost()
    {
        _speed *= 2;
        StartCoroutine(SpeedBoostLifeSpan());
    }
    public void DisableSpeedBoost()
    {
        _speed /= 2;
    }
    IEnumerator SpeedBoostLifeSpan()
    {
        yield return new WaitForSeconds(5f);
        DisableSpeedBoost();
    }

    // Shield powerup
    public void EnableShield()
    {
        _isShield = true;
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldLifeSpan());
    }
    public void DisableShield()
    {
        _isShield = false;
        _shieldVisualizer.SetActive(false);
    }
    IEnumerator ShieldLifeSpan()
    {
        yield return new WaitForSeconds(20f);
        DisableShield();
    }

    // Scoring
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}

