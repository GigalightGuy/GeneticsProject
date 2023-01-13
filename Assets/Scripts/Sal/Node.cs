using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> 
{
    public bool walkable; // Não é um obstaculo
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool _walkable, Vector3 _worldPosition, int gridX, int gridY)
    {
        this.walkable = _walkable;
        this.worldPosition = _worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
        
    }
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }
    public int CompareTo (Node nodeToCompare)
    {
        int compare= fCost.CompareTo (nodeToCompare.fCost);
        if(compare == 0) 
        {
            compare=hCost.CompareTo (nodeToCompare.hCost);
        }
        //Retornamos 1 se o item atual tiver uma prioridade mais alta do que item no qual estamos a comparar, entao como nossos "nós" que estão invertidos queremos retornar
        // um se for menor, então usamos o negativo 
        return - compare; 
    }
}
