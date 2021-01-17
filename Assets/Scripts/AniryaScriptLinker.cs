using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniryaScriptLinker : MonoBehaviour
{
    public CombatManager CombatM;
    public Player Player;
    public CharacterController2D CC2D;
    public CharacterMovement CM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckHit()
    {
        CombatM.CheckHit();
    }

    /*public void EnableLeftFlame()
    {
        CC2D.EnableLeftFlame();
    }
    public void EnableRightFlame()
    {
        CC2D.EnableRightFlame();
    }
    public void DisableLeftFlame()
    {
        CC2D.DisableLeftFlame();
    }

    public void DisableRightFlame()
    {
        CC2D.DisableRightFlame();
    }*/
}
