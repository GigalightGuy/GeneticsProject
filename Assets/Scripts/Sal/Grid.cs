using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize; // tamanho da gride
    public float nodeRadius;
    Node[,] grid; //Matriz X e Y

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    void Start()
    {
        //Calcula o tamanho de cada nodo
        nodeDiameter = nodeRadius * 2; 
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter); 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
                                    //Centro  - ... (Gera borda esquerda) - altura = borda esquerda inferior
        Vector3 worldBottomLeft= transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right *(x*nodeDiameter + nodeRadius) + Vector3.forward * (y *nodeDiameter + nodeRadius);
                //Retorna true se houver uma colisão, ou seja, se retornar true vira falso 
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                //Construimos a grid com base na variavel bool, ou seja, se houver obstaculo não há node!
                grid[x, y] = new Node(walkable, worldPoint);

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y)); //Utilziamos o Y no fim pq o Z representa a altura em 3D

        if(grid!=null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color= (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
