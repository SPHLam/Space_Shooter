using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 4.0f;

    // Handle to animator component
    private Animator _enemyAnimator;

    private Player _player;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 0.0f;
    private float _nextFire = 0.0f;
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 6, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is null!");
        }

        // assign the component to Anim
        _enemyAnimator = GetComponent<Animator>();
        if(_enemyAnimator == null)
        {
            Debug.LogError("Animator is null!");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio source on enemy is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if(Time.time > _nextFire)
        {
            _fireRate = Random.Range(3f, 6f);
            Fire(_fireRate);
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float x, y;
        if (transform.position.y <= -6f)
        {
            y = transform.position.y * -1;
            x = Random.Range(-9.5f, 9.5f);
        }
        else
        {
            y = transform.position.y;
            x = transform.position.x;
        }
        transform.position = new Vector3(x, y, transform.position.z);
    }

    void Fire(float fireRate)
    {
        _nextFire = Time.time + fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for(int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddScore(10);
            // trigger enemy death animation
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
        else if (other.tag == "Player")
        {
            Debug.Log("Player touched!");
            // trigger enemy death animation
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            if (_player != null)
            {
                _player.Damage();
            }
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }
}