using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCheking : MonoBehaviour
{
    FoodHandler foodHandler;
    Collider myCollider;
    public int currentPredatorsInside;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        foodHandler = GetComponentInParent<FoodHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Predator")) currentPredatorsInside++;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Predator")) currentPredatorsInside--;
    }

    public int ReturnPredatorCount()
    {
        return currentPredatorsInside;
    }
    public void SpawnNewFood(GameObject food)
    {
        Vector3 spawnPosition = RandomPositionInsideCollider();
        food.GetComponent<NavMeshAgent>().Warp(spawnPosition);
    }

    Vector3 RandomPositionInsideCollider()
    {
        return myCollider.bounds.center + new Vector3(
            (Random.value -0.5f) * myCollider.bounds.size.x,
            0,
            (Random.value -0.5f) * myCollider.bounds.size.z);          
    }
}
