using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWolf : EnnemyMovementController
{

    public List<GameObject> WanderNodes;
    public List<GameObject> accessibleNodes;
    public GameObject DestinationNode;
    public GameObject StartNode;
    public GameObject ActualNode;

    public GameObject parent;

    public float Heuristique = 1;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < WanderNodes.Count; i++)
        {
            accessibleNodes.Add(WanderNodes[i]);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.P))
        {
            AStar();
        }
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
        List<GameObject> activeNodes = new List<GameObject>();
        activeNodes.Add(StartNode);
        List<GameObject> visitedNodes = new List<GameObject>();

        ActualNode = StartNode;
        StartNode.GetComponent<NodeController>().Distance = Vector2.Distance(DestinationNode.transform.position, StartNode.transform.position);
        StartNode.GetComponent<NodeController>().Cost = 0;

        GameObject nodeToCheck = null;

        while (activeNodes.Count != 0)
        {
            nodeToCheck = null;
            for (int i = 0; i < activeNodes.Count; i++)
            {
                if (nodeToCheck == null || activeNodes[i].GetComponent<NodeController>().CostDistance <= nodeToCheck.GetComponent<NodeController>().CostDistance)
                {
                    nodeToCheck = activeNodes[i];
                }
            }

            visitedNodes.Add(nodeToCheck);
            activeNodes.Remove(nodeToCheck);

            List<GameObject> neighbours = GetNeighbourNodes(nodeToCheck);

            bool found = false;
            bool continued = false;

            foreach (GameObject node in neighbours)
            {
                //Checks if node already visited
                for (int i = 0; i < visitedNodes.Count; i++)
                {
                    if (node == visitedNodes[i])
                    {
                        continued = true;
                        continue;
                    }
                }

                if (continued)
                {
                    continued = false;
                    continue;
                }

                for (int i = 0; i < activeNodes.Count; i++)
                {
                    if (node == activeNodes[i])
                    {
                        found = true;
                        if(activeNodes[i].GetComponent<NodeController>().CostDistance  <= nodeToCheck.GetComponent<NodeController>().CostDistance)
                        {
                            activeNodes[i].GetComponent<NodeController>().Cost = activeNodes[i].GetComponent<NodeController>().TemporaryCost;
                        }
                    }
                }
                if (!found)
                {
                    found = false;
                    activeNodes.Add(node);
                }

            }



            if (nodeToCheck == DestinationNode)
            {
                Debug.Log("We are at the destination!");
                GameObject actualNode = StartNode;
                
                while(actualNode != DestinationNode)
                {
                    actualNode.GetComponent<CircleCollider2D>().radius = 1;
                    actualNode = actualNode.GetComponent<NodeController>().parent;
                }
                actualNode.GetComponent<CircleCollider2D>().radius = 1;
                //We can actually loop through the parents of each tile to find our exact path which we will show shortly. 
                break;
            }
        }
    }

    public List<GameObject> GetNeighbourNodes(GameObject nodeToCheck)
    {
        foreach (GameObject node in nodeToCheck.GetComponent<NodeController>().neighbours)
        {
            if(node.GetComponent<NodeController>().Distance == 0)
            {
                node.GetComponent<NodeController>().parent = nodeToCheck;
                node.GetComponent<NodeController>().Distance = Vector2.Distance(node.transform.position, DestinationNode.transform.position);
                node.GetComponent<NodeController>().Cost = nodeToCheck.GetComponent<NodeController>().Cost + Heuristique;
            }
            else
            {
                node.GetComponent<NodeController>().parent = nodeToCheck;
                node.GetComponent<NodeController>().Distance = Vector2.Distance(node.transform.position, DestinationNode.transform.position);
                node.GetComponent<NodeController>().TemporaryCost = nodeToCheck.GetComponent<NodeController>().Cost + Heuristique;
            }

            
        }
        return nodeToCheck.GetComponent<NodeController>().neighbours;
    }
}

