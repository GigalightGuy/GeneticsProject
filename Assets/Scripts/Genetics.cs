using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Genetics : MonoBehaviour
{

    public static Genetics genetics;
    [Header("Individual Mutation Values")]
    [SerializeField] float maxNegativeSizeMutation;
    [SerializeField] float maxPositiveSizeMutation;
    [SerializeField] float maxNegativeSpeedMutation;
    [SerializeField] float maxPositiveSpeedMutation;
    [SerializeField] float maxNegativeHungerResistanceMutation;
    [SerializeField] float maxPositiveHungerResistanceMutation;

    [Header("Size and Speed Impact on Hunger Resistance")]
    [SerializeField] float sizeImpact;
    [SerializeField] float speedImpact;


    [Header("Clamp Values (Optional)")]
    public float maxSizeFactor;
    public float minSizeFactor;
    public float maxSpeedFactor;
    public float minSpeedFactor;
    public float maxHungerResistance;
    public float minHungerResistance;


    private void Awake()
    {
        if (genetics != null)
        {
            Destroy(gameObject);
        }
        else genetics = this;
    }

    public float MutateSize(float sizeFactor)
    {
        float returnValue = sizeFactor + Random.Range(maxNegativeSizeMutation, maxPositiveSizeMutation);

        if (returnValue > maxSizeFactor) return maxSizeFactor;
        else if (returnValue < minSizeFactor) return minSizeFactor;
        else return returnValue;

    }

    public float MutateSpeed(float speedFactor)
    {
        float returnValue = speedFactor + Random.Range(maxNegativeSpeedMutation, maxPositiveSpeedMutation);

        if (returnValue > maxSpeedFactor) return maxSpeedFactor;
        else if (returnValue < minSpeedFactor) return minSpeedFactor;
        else return returnValue;
    }

    public float MutateHungerResistance(float hungerRes , float sizeFactor, float speedFactor)
    {
        float returnValue = 1 + Random.Range(maxNegativeHungerResistanceMutation, maxPositiveHungerResistanceMutation);
        returnValue /= ((sizeFactor + speedFactor) / 2);
        Debug.LogWarning("HungerResistance Factor = " + returnValue);
        if (returnValue > maxHungerResistance) return maxHungerResistance;
        else if (returnValue < minHungerResistance) return minHungerResistance;
        else return returnValue;
    }
}
