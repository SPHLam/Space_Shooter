using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Triple shot power up speed
    private float _speed = 3f;

    // Triple shot power up lifespan
    private float _lifeSpan;

    [SerializeField] // 0 - Triple shot, 1 - Speed, 2 - Shield
    private int _powerUpID;
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 6, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        // when we leave the screen, destroy this object
        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }

    }
    // on trigger collision
    // Only be collectable by the Player (HINT: use tags)
    // on collected, destroy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
            Player player = collision.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        player.EnableTripleShot();
                        break;
                    case 1:
                        player.EnableSpeedBoost();
                        break;
                    case 2:
                        player.EnableShield();
                        break;
                }
            }
        }
    }

}
