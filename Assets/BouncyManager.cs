using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyManager : MonoBehaviour
{
    [SerializeField]
    private float bouncy_amount = 26;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(collision.gameObject.tag == "Player")
        {
            Player.GetComponent<Animator>().SetBool("jump", false);
            Player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bouncy_amount, ForceMode2D.Impulse);
        }
    }
}
