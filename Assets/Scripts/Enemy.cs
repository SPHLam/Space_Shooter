using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 6, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
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
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            if(_player != null)
            {
                _player.Damage();
            }
        }
    }
}