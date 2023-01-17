using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;

    [SerializeField] private List<GameObject> animals = new List<GameObject>();
    private List<Animal> animalStats = new List<Animal>();

    [Header("TextBoxes")]
    [SerializeField] TextMeshProUGUI avgSizeTxt;
    [SerializeField] TextMeshProUGUI avgSpeedTxt;
    [SerializeField] TextMeshProUGUI avgHRTxt;
    [SerializeField] TextMeshProUGUI oldestGenTxt;
    [SerializeField] TextMeshProUGUI youngestGenTxt;
    [SerializeField] TextMeshProUGUI popMan;
    int deadElems = 0;
    int liveElems = 0;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }
    public void AddAnimal(GameObject animal)
    {
        animals.Add(animal);
        liveElems++;
        animalStats.Add(animal.GetComponent<Animal>());
        RecalculateStats();

    }
    public void RemoveAnimal(GameObject animal)
    {
        animals.Remove(animal);
        deadElems++;
        liveElems--;
        animalStats.Remove(animal.GetComponent<Animal>());
        RecalculateStats();
    }

    private void Update()
    {
    }

    public void RecalculateStats()
    {
        float avgSize = AverageSize();
        float avgSpeed = AverageSpeed();
        float avgHungerRes = AverageHungerRes();
        int oldestGen = CheckOldestGen();
        int youngestGen = CheckYoungestGen();


        ShowStats(avgSize, avgSpeed, avgHungerRes, oldestGen, youngestGen);
    }

    private int CheckYoungestGen()
    {
        int[] gens = new int[animalStats.Count];
        for (int i = 0; i < animalStats.Count; i++)
        {
            gens[i] = animalStats[i]._generation;
        }
        return gens.Min();
    }

    private int CheckOldestGen()
    {
        int[] gens = new int[animalStats.Count];
        for (int i = 0; i < animalStats.Count; i++)
        {
            gens[i] = animalStats[i]._generation;
        }
        return gens.Max();
    }

    private float AverageSize()
    {
        float sum = 0;
        foreach (var stat in animalStats)
        {
            sum += stat._sizeFactor;
        }
        return sum/animalStats.Count;
    }

    private float AverageSpeed()
    {
        float sum = 0;
        foreach (var stat in animalStats)
        {
            sum += stat._speedFactor;
        }
        return sum / animalStats.Count;
    }

    private float AverageHungerRes()
    {
        float sum = 0;
        foreach (var stat in animalStats)
        {
            sum += stat._hungerResistance;
        }
        return sum / animalStats.Count;
    }
    private void ShowStats(float size, float speed, float hungerRes, int oldest, int newest)
    {
        avgSizeTxt.text = size.ToString("#0.00");
        avgSpeedTxt.text = speed.ToString("#0.00");
        avgHRTxt.text = hungerRes.ToString("#0.00");
        oldestGenTxt.text = oldest.ToString();
        youngestGenTxt.text = newest.ToString();
        popMan.text = "Population Info:        " + " Live Elements: " + liveElems + "          Dead Elements: " + deadElems;
    }
}
