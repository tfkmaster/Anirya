using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Ennemy
{
    public List<GameObject> WanderNodes;
    public List<GameObject> accessibleNodes;
    public GameObject DestinationNode;
    public GameObject StoredNode;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < WanderNodes.Count; i++)
        {
            accessibleNodes.Add(WanderNodes[i]);
        }
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

    public void SelectANode()
    {
        DestinationNode = accessibleNodes[Random.Range(0, accessibleNodes.Count)];
    }
}
