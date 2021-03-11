//<<<<<<< Updated upstream:Assets/Scripts/AudioSettings.cs
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioSettings : MonoBehaviour
{
     void Awake () 
  {
      
  }
   void Update () 
  {
     
  }
   public void AniryaWalking()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Anirya Footsteps", GetComponent<Transform>().position);

    }
      public void AniryaJumpHead()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Jump", GetComponent<Transform>().position);


    }
    public void AniryaJumpTail()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Jump", GetComponent<Transform>().position);

    }
    public void AniryaHit()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Punch", GetComponent<Transform>().position);

    }
    public void AniryaHeal()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Heal", GetComponent<Transform>().position);

    }
    public void ShroomBump()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/FX environement/Bumper", GetComponent<Transform>().position);

    }

     public void WolfAttack()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Ennemies/Wolf attack", GetComponent<Transform>().position);

    }
}
/*
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioSettings : MonoBehaviour
{
     void Awake () 
  {
      
  }
   void Update () 
  {
     
  }
   public void AniryaWalking()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Anirya Footsteps", GetComponent<Transform>().position);

    }
      public void AniryaJumpHead()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Jump", GetComponent<Transform>().position);


    }
    public void AniryaJumpTail()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Jump", GetComponent<Transform>().position);

    }
    public void AniryaHit()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Punch", GetComponent<Transform>().position);

    }
    public void AniryaHeal()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Heal", GetComponent<Transform>().position);

    }
    public void ShroomBump()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/FX environement/Bumper", GetComponent<Transform>().position);

    }
}
>>>>>>> Stashed changes:Assets/AudioSettings.cs*/
