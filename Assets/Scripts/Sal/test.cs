using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float startValue = -0.5f;
    public float endValue = 1.5f;

    private float timeCount = 0.0f;

    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount > 1.0f)
        {
            float result = Random.value;
            result = result * (endValue - startValue);
            result = result + startValue;

            float clampValue = Mathf.Clamp01(result);

            Debug.Log("value: " + result.ToString("F3") + " result: " + clampValue.ToString("F3"));

            timeCount = 0.0f;
        }
    }
}  

