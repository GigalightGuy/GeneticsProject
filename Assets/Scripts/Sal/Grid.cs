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
    
    //public List<Node> GetNeighbours(Node node)
    //{
    //   List<Node> neighbours = new List<Node>();
    //    for (int x = -1; x <=1; x++)
    //    {
    //        for (int y = -1; y <=1; y++)
    //        {
    //            if (x == 0 && y == 0) continue;

    //            int checkX = node.gridX + x;
    //            int checkY = node.gridY + y;

    //            if(checkX >= 0 && checkX< gridSizeX && checkY >= 0 && checkY<gridSizeY)
    //                neighbours.Add(grid[checkX, checkY]);
    //        }
    //    }
    //}

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
                grid[x, y] = new Node(walkable, worldPoint,x,y);

            }
        }
    }
    /*
     *  Clamp
        Retorna o valor mínimo se o valor float fornecido for menor que o mínimo. Retorna o valor máximo se o valor 
        fornecido for maior que o valor máximo. 
        Use Clamp para restringir um valor a um intervalo definido pelos valores mínimo e máximo.
        Observação: se o valor mínimo for maior que o valor máximo, o método retorna o valor mínimo.

        Clamp01
        retorna o valor atual, caso seja negativo retorna 0, caso seja maior que 1, retonra 1.
     */
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percenteX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percenteY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        
        percenteX = Mathf.Clamp01(percenteX);
        percenteY = Mathf.Clamp01(percenteY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percenteX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percenteY);
        return grid[x, y];
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
