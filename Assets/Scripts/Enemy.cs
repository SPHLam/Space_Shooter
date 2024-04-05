using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 4.0f;
    void Start()
    {
        transform.position = new Vector3(0, 6, 0);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
        }
    }
}