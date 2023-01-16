using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    private Vector3 destination;

    public Vector3 Destination 
    { 
        get => destination; 
        set
        {
            destination = value;
            PathRequestManager.RequestPath(transform.position, destination, OnPathFound);
        }
    }

    public float speed = 1f;
    Vector3[] path;
    int targetIndex;

    //private void Start()
    //{
       
    //    PathRequestManager.RequestPath(transform.position, destination, OnPathFound);


    //}
    //private void Update()
    //{
        
    //    PathRequestManager.RequestPath(transform.position, destination, OnPathFound);
       
    //}
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
        targetIndex = 0;
        if (path.Length > 0)
        {
            Vector3 currentWayPoint = path[0];
            while (true)
            {
                if (transform.position == currentWayPoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length) yield break;
                    currentWayPoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}  

