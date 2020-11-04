using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWolf : MonoBehaviour
{

    public List<GameObject> WanderNodes;
    public List<GameObject> accessibleNodes;
    public GameObject DestinationNode;
    public GameObject ActualNode;

    public float Heuristique = 5;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < WanderNodes.Count; i++)
        {
            accessibleNodes.Add(WanderNodes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SelectANode()
    {
        int index = Random.Range(0, accessibleNodes.Count);
        if (DestinationNode)
        {
            accessibleNodes.Add(DestinationNode);
        }
        DestinationNode = accessibleNodes[index];
        accessibleNodes.RemoveAt(index);
    }

    public void AStar()
    {
        ActualNode = DestinationNode;
        DestinationNode.GetComponent<NodeController>().Distance = Vector2.Distance(DestinationNode.transform.position, gameObject.transform.position);
        DestinationNode.GetComponent<NodeController>().Cost = 0;
    }

    public void CalculateCost()
    {
        foreach (GameObject node in ActualNode.GetComponent<NodeController>().neighbours)
        {
            node.GetComponent<NodeController>().Distance = Vector2.Distance(node.transform.position, gameObject.transform.position);
            node.GetComponent<NodeController>().Cost = ActualNode.GetComponent<NodeController>().Cost + 1;
        }
    }
}
