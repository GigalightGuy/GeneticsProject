using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [Header("Species Variables")]
    public float _lifeTimeInHours;
    public float _defaultSpeed;
    public float _defaultFoodLossPerHour;
    public int _maxBabiesInLife;
    public int _maxFood;
    public int _foodOnBirth;
    public int _minFoodForHorny;
    public float _breedCooldownInHours;
    public float _breedCost;

    [Header("Personal Genetics")]
    public float _sizeFactor;
    public float _speedFactor;
    public float _hungerResistance;

    [Header("Life Variables")]
    public char _bloodLine;
    public int _generation;
    public float _currentFood;
    public float _foodLostPerHour;
    public int _numberOfDescendants;
    public GameObject _parent;
    public bool _readyToBreed = true;
    public bool _controlledBreeding = true;


    void Awake()
    {
        Destroy(gameObject,_lifeTimeInHours * 3600);
        _numberOfDescendants = 0;
        _currentFood = _foodOnBirth;
        _foodLostPerHour = _defaultFoodLossPerHour;
    }
    void Start()
    {
        PopulationManager.instance.AddAnimal(gameObject);
        gameObject.name = "wolfGen" + _generation + "_" + _bloodLine;
        StartCoroutine(ReadyBreeding());
    }

    public void Born(Animal parent)
    {
        _sizeFactor = Genetics.genetics.MutateSize(parent._sizeFactor);
        _speedFactor = Genetics.genetics.MutateSpeed(parent._speedFactor);
        _hungerResistance = Genetics.genetics.MutateHungerResistance(parent._hungerResistance,_sizeFactor, _speedFactor);
        _readyToBreed = false;
        _parent = parent.gameObject;

        ApplyChanges();
        //Destroy(parent.gameObject); // Tests ONLY
    }

    private void ApplyChanges()
    {
        gameObject.transform.localScale = Vector3.one * _sizeFactor;
        if(gameObject.GetComponent<NavAgent>() != null) gameObject.GetComponent<NavAgent>().speed = _defaultSpeed * _speedFactor;
        else if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            gameObject.GetComponent<NavMeshAgent>().speed = _defaultSpeed * _speedFactor;
            gameObject.GetComponent<NavMeshAgent>().acceleration = 8 * _speedFactor;
        }
        _foodLostPerHour *= (2 - _hungerResistance);
    }

    public void LoseFood(float forcedLoss = 0)
    {
        _currentFood -= _foodLostPerHour /3600 * Time.deltaTime + forcedLoss;
    }

    public void GainFood(float amount)
    {
        _currentFood += amount;
        if (_currentFood >= _maxFood) _currentFood = _maxFood;
    }
    
    public void Breed()
    {
        if (_numberOfDescendants >= _maxBabiesInLife) return;
        else if (_currentFood <= _minFoodForHorny) return;

        Debug.LogWarning("FILHO FILHO FILHO");
        GameObject descendant = Instantiate(gameObject, transform.position - transform.forward, Quaternion.identity);
        var descendantScript = descendant.GetComponent<Animal>();
        descendantScript.Born(this);
        descendantScript._generation = _generation + 1;
        descendant.transform.parent = transform.parent;
        _numberOfDescendants++;
        LoseFood(_breedCost);
        _readyToBreed = false;
        // StartCoroutine(ReadyBreeding());
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentFood <= 0) Destroy(gameObject);
        else LoseFood();

        //Test Poop Baby
        if (Input.GetKeyDown(KeyCode.B))
        {
            Breed();
        }
        if (_readyToBreed && _controlledBreeding)
        {
            Breed();
        }
    }

    IEnumerator ReadyBreeding()
    {
        yield return new WaitForSeconds(_breedCooldownInHours*3600);
        _readyToBreed = true;
    }

    private void OnDestroy()
    {
        PopulationManager.instance.RemoveAnimal(gameObject);
    }
}
