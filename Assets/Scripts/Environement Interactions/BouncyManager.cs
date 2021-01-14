using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyManager : MonoBehaviour
{
    [SerializeField]
    private float bouncy_amount = 6f;
    [SerializeField]
    private float bump_direction_x;
    [SerializeField]
    private float bump_direction_y;

    public GameObject Player;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        bump_direction_x = 1f;
        bump_direction_y = 1f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player colliding");
            GetComponent<Animator>().SetTrigger("bounce");
            Player.GetComponentInChildren<Animator>().SetBool("jump", true);

            //Player.GetComponent<CharacterMovement>().SetVelocity(new Vector2(1f, bouncy_amount));

            //Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Player.GetComponent<Rigidbody2D>().AddForce(new Vector2(bump_direction_x, bump_direction_y) * bouncy_amount, ForceMode2D.Impulse);
        }
    }
}
