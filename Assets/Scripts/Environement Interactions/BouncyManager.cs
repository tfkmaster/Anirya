using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyManager : MonoBehaviour
{
    [SerializeField]
    private float bouncy_amount = 26f;
    [SerializeField]
    private float bump_direction_x;
    [SerializeField]
    private float bump_direction_y;

    void Start()
    {
        bump_direction_x = 1f;
        bump_direction_y = 1f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(collision.gameObject.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("bounce");
            Player.GetComponentInChildren<Animator>().SetBool("jump", true);
            Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(bump_direction_x, bump_direction_y) * bouncy_amount, ForceMode2D.Impulse);
        }
    }
}
