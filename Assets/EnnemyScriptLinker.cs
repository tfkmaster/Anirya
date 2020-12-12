using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyScriptLinker : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamages()
    {
        GetComponentInParent<Wolf>().DealDamages();
    }

    public void StartFade()
    {
        GetComponentInParent<Wolf>().StartFade();
    }
}
