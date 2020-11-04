using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public List<GameObject> neighbours = new List<GameObject>();
    public float Cost;
    public float Distance;
    public float CostDistance => Cost + Distance;
    public GameObject parent;

    [SerializeField] private LayerMask WhatIsGround;                          // A mask determining what is ground to detect further raycast collision
    [SerializeField] private LayerMask WhatIsNodeLayer;                          // A mask determining in which layer the nodes are

    public float collisionRadius = 0.12f;
    private Color debugCollisionColor = Color.blue;

    public Collider2D[] test;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.5f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        test = Physics2D.OverlapCircleAll(transform.position, collisionRadius, WhatIsNodeLayer);

        foreach (Collider2D col in test)
        {
            if(col.gameObject != gameObject)
            {
                neighbours.Add(col.gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugCollisionColor;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}
