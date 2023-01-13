using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable; // Não é um obstaculo
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
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
}
