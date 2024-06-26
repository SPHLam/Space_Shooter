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
    private float _nextFirePlayerOne = 0.0f;
    private float _nextFirePlayerTwo = 0.0f;

    // Enemy spawn manager
    private SpawnManager _spawnManager;

    // Power up condition
    private bool _isTripleShot = false;
    private bool _isShield = false;

    // Variable reference to the shield visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;

    // Player's lives
    [SerializeField]
    private int _lives = 3;

    // Score
    [SerializeField]
    private int _score = 0;
    private int _bestScore;

    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;

    [SerializeField]
    private bool _isPlayerOne = false;
    [SerializeField]
    private bool _isPlayerTwo = false;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        // If CO-OP Mode, do nothing
        if (!_gameManager._isCoOpMode)
        {
            // transform = object
            // Take the current position = new position (0, 0, 0)
            transform.position = new Vector3(0, 0, 0);
        }

        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio source on the player is null!");
        }

        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerOne)
        {
            CalculatePlayerOneMovement();
            // Hit the space key to spawn laser object & checking delay for firing
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFirePlayerOne)
            {
                FireLaser();
            }
        }
        else if (_isPlayerTwo)
        {
            CalculatePlayerTwoMovement();
            if(Input.GetKeyDown(KeyCode.KeypadEnter) && Time.time > _nextFirePlayerTwo)
            {
                FireLaser();
            }
        }
    }
    void CalculatePlayerOneMovement()
    {
        // Vector3.left = new Vector3(1, 0, 0);
        // Time.deltaTime : 1s;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        // Movement wasd
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        // Move outside the screen will bring the Player to the opposite side
        var x = Mathf.Abs(transform.position.x) >= 12 ? transform.position.x * -1 : transform.position.x;
        var y = Mathf.Abs(transform.position.y) >= 7 ? transform.position.y * -1 : transform.position.y;
        transform.position = new Vector3(x, y, 0);
    }

    void CalculatePlayerTwoMovement()
    {
        // Keypad 8 to move up
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        // Keypad 2 to move down
        if (Input.GetKey(KeyCode.Keypad2))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        // Keypad 4 to move right
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        // Keypad 6 to move left
        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        var x = Mathf.Abs(transform.position.x) >= 12 ? transform.position.x * -1 : transform.position.x;
        var y = Mathf.Abs(transform.position.y) >= 7 ? transform.position.y * -1 : transform.position.y;
        transform.position = new Vector3(x, y, 0);
    }

    void FireLaser()
    {
        if (_isPlayerOne)
        {
            _nextFirePlayerOne = Time.time + _fireRate; // Add cooldown delay 
        }
        else if(_isPlayerTwo)
        {
            _nextFirePlayerTwo = Time.time + _fireRate;
        }
        if (!_isTripleShot)
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z), Quaternion.identity);
        } 
        else
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        // Play the audio clip 
        _audioSource.clip = _laserSoundClip;
        _audioSource.Play();
    }

    // Damaging feature
    public void Damage()
    {
        if (!_isShield)
        {
            _lives--;
            if (_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            if (_lives == 1)
            {
                _leftEngine.SetActive(true);
            }
            // Updating UI
            _uiManager.UpdateLives(_lives);
            if (_lives < 1)
            {
                _spawnManager.StopSpawning();
                _uiManager.ShowGameOver();
                CheckForBestScore();
                Destroy(this.gameObject);
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

    private void CheckForBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("Best Score", _bestScore);
            _uiManager.UpdateBestScore(_bestScore);
        }
    }
}

