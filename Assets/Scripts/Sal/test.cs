using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target;
    [SerializeField]float speed = 1f;
    Vector3[] path;
    int targetIndex;
    Vector3 targetLastPos;

    private void Start()
    {
        Debug.Log("Entrei no start");
        PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);


    }
    private void Update()
    {
        if (target.position != targetLastPos)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
        targetLastPos = target.position;
    }
    public void OnPathFound(Vector3[] newPath,bool pathSucessful)
    {
        if (pathSucessful)
        {
            path= newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator FollowPath() 
    {
        Vector3 currentWayPoint = path[0];
        while(true)
        {
            if(transform.position == currentWayPoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length) yield break;
                currentWayPoint = path[targetIndex];
            }
           transform.position= Vector3.MoveTowards(transform.position,currentWayPoint,speed);
            yield return null;
        }
    }
}  

