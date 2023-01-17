using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgentTest : MonoBehaviour
{
    [SerializeField] Transform[] target;
    private Vector3 destination;
    int count = 0;
    public Vector3 Destination
    {
        get => destination;
        set
        {
            destination = value;
            
           selfPathfinding.FindPath(transform.position, destination,OnPathFound);
            
        }
    }
    public SelfPathfinding selfPathfinding;
    public float speed = 1f;
    Vector3[] path;
    int targetIndex;


    private void Awake()
    {
        selfPathfinding= GetComponent<SelfPathfinding>();
    }
    //private void Start()
    //{

    //    PathRequestManager.RequestPath(transform.position, destination, OnPathFound);


    //}
    private void Update()
    {
        if (Input.GetKey(KeyCode.M) && target != null)
        {
            //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);


            Destination = target[count].position;
            count = (count + 1) % target.Length;
        }


    }


    public void OnPathFound(Vector3[] newPath, bool pathSucessful)
    {
        if (pathSucessful)
        {
            path = newPath;
            //if ((path[path.Length - 1] - Destination).sqrMagnitude < 4f)
            //{
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            //}
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
                if (Vector3.SqrMagnitude(transform.position - currentWayPoint) < 0.1f)
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
