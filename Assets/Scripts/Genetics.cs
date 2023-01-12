using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetics : MonoBehaviour
{
    [SerializeField][Range(-1, 1)] float sizeFactor;
    [SerializeField][Range(-1, 1)] float coldResistance;
    [SerializeField][Range(-1, 1)] float heatResistance;
    [SerializeField][Range(-1, 1)] float speedFactor;
    [SerializeField][Range(-1, 1)] float hungerResistance;
    [SerializeField][Range(-1, 1)] float reproductionFactor;
    [SerializeField][Range(-1, 1)] float visionFactor;

    private Vector3 startScale;

    private void Birth(
    #region Genetic Parameters 
        float _sizeFactor,
        float _coldResistance,
        float _heatResistance,
        float _speedFactor,
        float _hungerResistance,
        float _reproductionFactor,
        float _visionFactor
    #endregion
        )
    {

    }

}
