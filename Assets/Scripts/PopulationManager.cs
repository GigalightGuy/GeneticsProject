using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> predators = new List<GameObject>();
    [SerializeField] private List<GameObject> prey = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var predator in GameObject.FindGameObjectsWithTag("Predator"))
        {
            predators.Add(predator);
        }
        foreach (var _prey in GameObject.FindGameObjectsWithTag("Prey"))
        {
            prey.Add(_prey);
        }
    }


    public void AddPredator()
    {

    }

    public void AddPrey()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
