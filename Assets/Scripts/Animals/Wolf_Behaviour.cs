using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Behaviour : Behaviour
{
    [SerializeField] float detectionRadius;
    [SerializeField] float biteRadius;
    [SerializeField] GameObject sensoryRadius;

    // Start is called before the first frame update
    void Start()
    {
        sensoryRadius.transform.localScale = new Vector3 (detectionRadius,0.01f,detectionRadius);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        FindPrey();
        if (currentTarget != null) TryToEatPrey();



    }

    private void FindPrey()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var possibleTarget in possibleTargets)
        {
            if (possibleTarget.CompareTag("Prey"))
            {
                gotTarget = true;
                currentTarget = possibleTarget.transform.gameObject;
                break;
            }
        }
    }

    private void TryToEatPrey()
    {
        Collider[] prey = Physics.OverlapSphere(transform.position, biteRadius);

        foreach (var _prey in prey)
        {
            if (_prey.gameObject == currentTarget)
            {
                EatPrey();
            }
        }
    }

    private void EatPrey()
    {
        currentTarget.GetComponent<Behaviour>().GetEaten();
        gameObject.GetComponent<Animal>().GainFood(30);
        currentTarget = null;
        gotTarget = false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 0, 0, .5f);
    //    Gizmos.DrawSphere(transform.position, biteRadius);
    //}
}
