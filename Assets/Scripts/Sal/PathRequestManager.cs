using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    /*
     * Iremos armazenar o caminho recebido e o chamaremos assim que tivemos realmente calculado o seu caminho, sendo assim, armazena-lo em uma ação
     * Action Vector 3 para o caminho real 
     * Action bool se o caminho era ou não o objetivo e se teve sucesso
     *
     */
    //Fila que solicita os caminhos 
    Queue<PathResquest> pathResquestsQueue=new Queue<PathResquest>();
    //Caminho atual
    PathResquest currentPathResquest;

    public static PathRequestManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;

    private void Awake()
    {
        if(instance != null) Destroy(gameObject);
        else instance = this; 
        pathFinding = GetComponent<PathFinding>();
    }
    public static void RequestPath(Vector3 parthStart, Vector3 pathEnd, Action<Vector3[],bool> callback)
    {
        if( callback == null )Debug.Log("callback null");
        if (instance == null) Debug.Log("Instance null");
        
        PathResquest newResquest= new PathResquest(parthStart,pathEnd,callback);
        
        instance.pathResquestsQueue.Enqueue(newResquest);
       


        instance.TryProcessNext();
    }

    public void TryProcessNext()
    {
        if(!isProcessingPath && pathResquestsQueue.Count >0)
        {
            //CurrentPathResquest passa para o primeiro item na fila 
            currentPathResquest= pathResquestsQueue.Dequeue();
            isProcessingPath= true;
            pathFinding.StartFindPath(currentPathResquest.parthStart,currentPathResquest.pathEnd);
        }
    }
    public void FinishProcessingPath(Vector3[] path, bool sucess) 
    {
        currentPathResquest.callback(path, sucess);
        isProcessingPath= false;
        TryProcessNext();
    }
    struct PathResquest
    {
        public Vector3 parthStart;
        public Vector3 pathEnd;
        public Action<Vector3[],bool> callback;

        public PathResquest(Vector3 start,Vector3 end, Action<Vector3[],bool> callBack)
        {
            parthStart= start;
            pathEnd= end;
            callback= callBack;
        }
    }
}
