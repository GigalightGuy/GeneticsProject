using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodHandler : MonoBehaviour
{
    public static FoodHandler instance;

    [SerializeField] List<GameObject> foods = new List<GameObject>();
    public float respawnCooldownInHours;
    SpawnCheking[] spawnCheckers = new SpawnCheking[4];

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }
    void Start()
    {
        for (int i = 0; i < spawnCheckers.Length; i++)
        {
            spawnCheckers[i] = transform.GetChild(i).GetComponent<SpawnCheking>();
        }
    }

    public void ReplaceMe(GameObject food)
    {
        StartCoroutine(RespawnFood(food));
    }
    IEnumerator RespawnFood(GameObject food)
    {
        yield return new WaitForSeconds(respawnCooldownInHours * 3600);
        SpawnCheking bestChecker;
        int index = 0;
        int minValue = int.MaxValue;
        for (int i = 0; i < spawnCheckers.Length; i++)
        {
            if (spawnCheckers[i].currentPredatorsInside <= minValue)
            {
                minValue = spawnCheckers[i].currentPredatorsInside;
                index = i;
            }
        }
        bestChecker = spawnCheckers[index];
        Debug.Log(bestChecker.gameObject.name);
        bestChecker.SpawnNewFood(food);
    }
}
