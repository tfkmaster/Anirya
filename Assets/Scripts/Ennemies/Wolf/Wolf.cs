using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Ennemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void OnHit(GameObject hitter, int damages)
    {
        base.OnHit(hitter, damages);
    }

    protected override void Death()
    {
        base.Death();
        Debug.Log("aa");
        GetComponentInChildren<Animator>().SetBool("dead", true);
    }

}
