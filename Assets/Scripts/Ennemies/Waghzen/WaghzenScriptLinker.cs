using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaghzenScriptLinker : MonoBehaviour
{
    private AIWaghzen AIWaghzen;

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
        gameObject.GetComponentInParent<Transform>().position -= new Vector3(gameObject.GetComponentInParent<Transform>().position.x, gameObject.GetComponentInParent<Transform>().position.y -7, gameObject.GetComponentInParent<Transform>().position.z);
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
}
