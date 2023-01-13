using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void FindPath(Vector3 starPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(starPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>(); // The set of nodes to be evalauted
        HashSet<Node> closeSet = new HashSet<Node>(); // the set of nodes already evalauted
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //Node in Open with the lowest fCost
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode);

            if (currentNode == targetNode) return;
        }
    }
}
