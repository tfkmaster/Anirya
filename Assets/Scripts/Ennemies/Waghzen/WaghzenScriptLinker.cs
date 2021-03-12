using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaghzenScriptLinker : MonoBehaviour
{
    private AIWaghzen AIWaghzen;

    public AudioClip GroundAttackClip;
    public AudioClip FallOnGroundGrowlClip;
    public AudioClip JumpGrowlClip;
    public AudioClip DieGrowlClip;

    // Start is called before the first frame update
    void Start()
    {
        AIWaghzen = GetComponentInParent<AIWaghzen>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        //gameObject.GetComponentInParent<Transform>().position = new Vector3(gameObject.GetComponentInParent<Transform>().position.x, gameObject.GetComponentInParent<Transform>().position.y - 40, gameObject.GetComponentInParent<Transform>().position.z);
    }

    public void BeforeJump()
    {
        AIWaghzen.GravityBeforeJump();
    }

    public void AfterJump()
    {
        AIWaghzen.GravityAfterJump();
    }

    public void StartJumpAction()
    {
        AIWaghzen.startJumpAction();
    }

    public void RockFall()
    {
        AIWaghzen.FallingRockManager.InstantiateRocks();
    }

    public void GroundAttack()
    {
        GetComponent<AudioSource>().clip = GroundAttackClip;
        GetComponent<AudioSource>().pitch = 1f;
        GetComponent<AudioSource>().Play();
    }

    public void Walk()
    {
        GetComponent<AudioSource>().clip = GroundAttackClip;
        GetComponent<AudioSource>().pitch = 0.2f;
        GetComponent<AudioSource>().Play();
    }

    public void StandUp()
    {
        GetComponent<AudioSource>().clip = GroundAttackClip;
        GetComponent<AudioSource>().pitch = 0.1f;
        GetComponent<AudioSource>().Play();
    }

    public void JumpAttack()
    {
        GetComponent<AudioSource>().clip = GroundAttackClip;
        GetComponent<AudioSource>().pitch = 0.3f;
        GetComponent<AudioSource>().Play();
    }

    public void FallOnGroundGrowl()
    {
        GetComponent<AudioSource>().clip = FallOnGroundGrowlClip;
        GetComponent<AudioSource>().pitch = 1f;
        GetComponent<AudioSource>().Play();
    }

    public void JumpGrowl()
    {
        GetComponent<AudioSource>().clip = JumpGrowlClip;
        GetComponent<AudioSource>().pitch = 1f;
        GetComponent<AudioSource>().Play();
    }

    public void DieGrowl()
    {
        GetComponent<AudioSource>().clip = DieGrowlClip;
        GetComponent<AudioSource>().priority = 20;
        GetComponent<AudioSource>().pitch = 0.8f;
        GetComponent<AudioSource>().Play();
    }
}
