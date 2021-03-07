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

    public void BeforeJump()
    {
        AIWaghzen.GravityBeforeJump();
    }

    public void AfterJump()
    {
        AIWaghzen.GravityAfterJump();
    }
}
