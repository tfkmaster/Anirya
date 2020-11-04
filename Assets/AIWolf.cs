using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWolf : MonoBehaviour
{

    public List<GameObject> WanderNodes;
    public List<GameObject> accessibleNodes;
    public GameObject DestinationNode;
    public GameObject StartNode;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            AStar();
        }
    }


    public void SelectANode()
    {
        /*int index = Random.Range(0, accessibleNodes.Count);
        if (DestinationNode)
        {
            accessibleNodes.Add(DestinationNode);
        }
        DestinationNode = accessibleNodes[index];
        accessibleNodes.RemoveAt(index);*/
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
            int checkedTileIndex = 0;
            for (int i = 0; i < activeNodes.Count; i++)
            {
                if (nodeToCheck == null || activeNodes[i].GetComponent<NodeController>().CostDistance <= nodeToCheck.GetComponent<NodeController>().CostDistance)
                {
                    nodeToCheck = activeNodes[i];
                    checkedTileIndex = i;
                }
            }

            visitedNodes.Add(nodeToCheck);
            activeNodes.RemoveAt(checkedTileIndex);

            List<GameObject> neighbours = GetNeighbourNodes(nodeToCheck);


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
            bool found = false;

            foreach (GameObject node in neighbours)
            {
                //Checks if node already visited
                for (int i = 0; i < visitedNodes.Count; i++)
                {
                    if (node.transform.position == visitedNodes[i].transform.position)
                    {
                        continue;
                    }
                }

                for (int i = 0; i < activeNodes.Count; i++)
                {
                    if (node.transform.position == activeNodes[i].transform.position)
                    {
                        found = true;
                        if(activeNodes[i].GetComponent<NodeController>().CostDistance > node.GetComponent<NodeController>().CostDistance)
                        {
                            activeNodes.RemoveAt(i);
                            activeNodes.Add(node);
                        }
                    }
                }
                if (!found)
                {
                    activeNodes.Add(node);
                }

            }
        }
    }

    public List<GameObject> GetNeighbourNodes(GameObject nodeToCheck)
    {
        foreach (GameObject node in nodeToCheck.GetComponent<NodeController>().neighbours)
        {
            node.GetComponent<NodeController>().parent = nodeToCheck;
            node.GetComponent<NodeController>().Distance = Vector2.Distance(node.transform.position, DestinationNode.transform.position);
            node.GetComponent<NodeController>().Cost = nodeToCheck.GetComponent<NodeController>().Cost + 1;
        }
        return nodeToCheck.GetComponent<NodeController>().neighbours;
    }
}

/*
 def find_path(self, start, end):
    tstart = time.time()
    count = 0
    openSet = set()
    closedSet = set([start])
    start.g_score(start)
    start.h_score(end)
    start.f_score()

    while len(closedSet) != 0:
        current = min(closedSet, key=attrgetter('f'))     
        closedSet.remove(current)

        for neighbour in current.neighbours:
            if neighbour.full or neighbour in openSet: continue
            if neighbour in closedSet:
                if neighbour.parent != None and neighbour.parent.g > current.g:
                    neighbour.parent = current
                    neighbour.g_score(start)
                    neighbour.f_score()
            else:

                neighbour.parent = current

                neighbour.g_score(start)
                neighbour.h_score(end)
                neighbour.f_score()
                closedSet.add(neighbour)

                if neighbour == end:
                    path = []
                    while end != None:
                        path.insert(0, end)
                        end = end.parent
                    return path

            openSet.add(current)

    return None
     */
