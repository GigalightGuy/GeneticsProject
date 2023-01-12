using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class Behaviour : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;
    public TextMeshProUGUI currentSpeed;
    [HideInInspector] public bool gotTarget = false;
    [HideInInspector] public GameObject currentTarget;

    private Transform target;
    private NavMeshAgent agent;
    private float initialSpeed;
    private float timer;
    private float timeScale;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer; 
        initialSpeed = agent.speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timeScale = TimeManager.instance.customTimeScale;
        timer += Time.deltaTime * timeScale;

        currentSpeed.text = agent.speed.ToString();
        Debug.Log(initialSpeed);

        if (timer >= wanderTimer && !gotTarget)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
        else if (gotTarget)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    public void ChangeSpeed()
    {
        agent.ResetPath();
        agent.speed = initialSpeed * timeScale;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
