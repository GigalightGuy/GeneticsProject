using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.MemoryProfiler;
using System;

public class PathFinding : MonoBehaviour
{
    PathRequestManager resquestManager;
    Grid grid;
    
    private void Awake()
    {
        resquestManager= GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }


    public void StartFindPath(Vector3 startPos,Vector3 targePos)
    {
        StartCoroutine(FindPath(startPos, targePos));
    }
     IEnumerator FindPath(Vector3 starPos, Vector3 targetPos)
    {
        Stopwatch sw= new Stopwatch();
        sw.Start();

        //Depois de esperar por um quadro, queremos chamar o processamento final que leva em uma matriz de vetor3 para o caminho
        Vector3[] wayPoints = new Vector3[0];
        bool pathSucess=false;

        Node startNode = grid.NodeFromWorldPoint(starPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            //Sem otimização 
            /*
             * List<Node> openSet= new List<Node>();
             */
            //Com otimização
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // The set of nodes to be evalauted
            HashSet<Node> closeSet = new HashSet<Node>(); // the set of nodes already evalauted
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                //Sem otimização
                // Node currentNode= openSet[0];
                //Com otimização
                Node currentNode = openSet.RemoveFirst();

                //Sem otimização

                /*for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) currentNode = openSet[i];
                }

                openSet.Remove(currentNode);
                */

                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path foind:" + sw.ElapsedMilliseconds + "ms");
                    pathSucess = true;

                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closeSet.Contains(neighbour)) continue;

                    //Calculamos o custo do node vizinho e a sua distancia.
                    int newMovimentCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovimentCostToNeighbour < neighbour.gCost || !openSet.Contans(neighbour))
                    {
                        neighbour.gCost = newMovimentCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);

                        neighbour.parent = currentNode;

                        if (!openSet.Contans(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        
        if(pathSucess) wayPoints = RetracePath(startNode, targetNode);

        resquestManager.FinishProcessingPath(wayPoints, pathSucess);
    }
    //Recaucula o path
    Vector3[] RetracePath(Node startNode,Node endNode) 
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) 
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] wayPoints = SimplifyPath(path,startNode);
        Array.Reverse(wayPoints);
        return wayPoints;
        
    }
    //Os pontos de referencia só são colocados aonde o caminho muda de direção
    //Vector3[] SimplifyPath(List<Node> path)
    //{
    //    List<Vector3> wayPoints = new List<Vector3>();
    //    Vector2 directionOld= Vector2.zero;
    //    for (int i = 1; i < path.Count; i++)
    //    {

    //        Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
    //        if(directionNew !=directionOld) 
    //        {
    //            wayPoints.Add(path[i].worldPosition);
    //        }
    //        directionOld= directionNew;
    //    }

    //    return wayPoints.ToArray();
    //}

    Vector3[] SimplifyPath(List<Node> path, Node startNode)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPosition); //Changed from path[i] to path[i-1]
            }
            directionOld = directionNew;
            if (i == path.Count - 1 && directionOld != new Vector2(path[i].gridX, path[i].gridY) - new Vector2(startNode.gridX, startNode.gridY))
                waypoints.Add(path[path.Count - 1].worldPosition);
        }
        return waypoints.ToArray();
    }
    int GetDistance(Node nodeA,Node nodeB)
    {
        //Calculamos a distancia entre os dois nodos
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        /*
         * A ideia principal é que se as coordenadas do nó inicial forem x1,y1 e as coordenadas do nó final forem x2,y2. 
         * Subtraindo x2 de x1 e y2 de y1 e calculando o valor absoluto de cada diferença, temos dois números. 
         * O menor número indica o número de movimentos diagonais onde cada movimento diagonal distance=root(1^2+1^2) =root(2) quase igual a 1,4 para facilitar a representação, 
         * multiplique-o por 10, então é 14 . O maior número indica o número de movimentos horizontais ou verticais necessários para chegar ao destino após os movimentos diagonais, 
         * pois cada movimento horizontal ou vertical equivale a uma distância de 1, depois multiplique por 10 para ser consistente.
         */
        if (dstX >dstY) return 14*dstY + 10*(dstX-dstY);
        return 14*dstX+ 10*(dstY-dstX);
    }
}
