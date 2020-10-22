using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform bottomMax;
    public Transform topMax;
    [SerializeField] private int speed = 10;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y <= bottomMax.position.y)
        {
            direction = 1;
        }
        if (transform.position.y >= topMax.position.y)
        {
            direction = -1;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime * direction, transform.position.z);
    }
}
