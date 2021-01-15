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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Colliding");
        if (collider.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("bounce");
            Player.GetComponentInChildren<Animator>().SetBool("jump", true);
            Player.GetComponent<CharacterMovement>().setVelocity(25);
        }
    }
}
