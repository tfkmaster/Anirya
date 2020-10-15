using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private Vector2 knockBackForce = new Vector2(1,1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnHit(GameObject hitter,  int damages)
    {
        if(hitter.GetComponent<Transform>().position.x > GetComponent<Transform>().position.x)
        {
            GetComponent<Rigidbody2D>().AddForce(knockBackForce * new Vector2(-1, 1), ForceMode2D.Impulse);
            //GetComponent<Rigidbody2D>().velocity += new Vector2(1, 0) * 30;
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(knockBackForce, ForceMode2D.Impulse);
            //GetComponent<Rigidbody2D>().velocity += new Vector2(-1, 0) * 30;
        }
        base.OnHit(hitter,damages);

    }

    protected void Death()
    {

    }
}
