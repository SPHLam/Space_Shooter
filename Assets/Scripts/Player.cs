using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed = 3.5f;
    void Start()
    {
        // transform = object
        // Take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();   
    }
    void CalculateMovement()
    {
        // Vector3.left = new Vector3(1, 0, 0);
        // Time.deltaTime : 1s;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);

        var x = Mathf.Abs(transform.position.x) >= 12 ? transform.position.x * -1 : transform.position.x;
        var y = Mathf.Abs(transform.position.y) >= 7 ? transform.position.y * -1 : transform.position.y;
        transform.position = new Vector3(x, y, 0);

        
    }
}
