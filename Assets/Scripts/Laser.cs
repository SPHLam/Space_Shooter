using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 10f;

    private bool _isEnemyLaser = false;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // Destroying objects
        if (transform.position.y >= 7)
        {
            // Check if this object has a parent
            // Destroy the parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Destroying objects
        if (transform.position.y <= -6)
        {
            // Check if this object has a parent
            // Destroy the parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
}
