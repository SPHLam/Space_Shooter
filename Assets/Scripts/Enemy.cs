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
    }

    // Update is called once per frame
    void Update()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddScore(10);
            // trigger enemy death animation
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.5f);
        }
        else if (other.tag == "Player")
        {
            // trigger enemy death animation
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.5f);
            if(_player != null)
            {
                _player.Damage();
            }
        }
    }
}