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
            GetComponent<Animator>().SetTrigger("bounce");
            Player.GetComponentInChildren<Animator>().SetBool("jump", true);
            Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bouncy_amount, ForceMode2D.Impulse);
        }
    }
}
