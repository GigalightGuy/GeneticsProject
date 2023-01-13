using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{

    [SerializeField] [Range(.1f, 100)] private float unityTimeScale = 100.0f;
    [SerializeField] private TextMeshProUGUI[] timeSlots;

    public static TimeManager instance;

    private float currentElapsedTime;
    [SerializeField] private TextMeshProUGUI currentTimeScale;


    private float seconds;
    private int minutes;
    private int hours;
    private int days;
    private int months;
    private int years;

    private bool readyInput = true;
    private bool stopped = false;
    private float saveValue;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = unityTimeScale;
        currentElapsedTime += Time.deltaTime;

        // if (Input.GetKey(KeyCode.M)) IncreaseTimeScale();
        // else if (Input.GetKey(KeyCode.N)) DecreaseTimeScale();

        if (Input.GetKey(KeyCode.RightArrow) && readyInput && unityTimeScale < 100)
        {
            if (unityTimeScale == 1.1f) unityTimeScale = 1;
            readyInput = false;
            unityTimeScale++;
            StartCoroutine(ReadyInput());
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && readyInput && unityTimeScale >= .1f)
        {
            readyInput = false;
            unityTimeScale--;
            if (unityTimeScale <= .1f) unityTimeScale = .1f;
            StartCoroutine(ReadyInput());
        }
        if (Input.GetKeyDown(KeyCode.Space) && !stopped)
        {
            saveValue = unityTimeScale;
            unityTimeScale = 0;
            stopped = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && stopped)
        {
            unityTimeScale = saveValue;
            stopped = false;
        }

        CalculateTimes();
        WriteTimes();
    }

    private IEnumerator ReadyInput()
    {
        yield return new WaitForSeconds(.1f * unityTimeScale);
        readyInput = true;
    }
    //private void IncreaseTimeScale()
    //{
    //    customTimeScale++;
    //    foreach (var wolf in predators)
    //    {
    //        wolf.GetComponent<Behaviour>().ChangeSpeed();
    //    }
    //    foreach (var cow in prey)
    //    {
    //        cow.GetComponent<Behaviour>().ChangeSpeed();
    //    }
    //}
    //private void DecreaseTimeScale()
    //{
    //    customTimeScale--;
    //    foreach (var wolf in predators)
    //    {
    //        wolf.GetComponent<Behaviour>().ChangeSpeed();
    //    }
    //    foreach (var cow in prey)
    //    {
    //        cow.GetComponent<Behaviour>().ChangeSpeed();
    //    }
    //}

    private void CalculateTimes()
    {
        seconds = (int)(currentElapsedTime % 60);
        minutes = (int)(currentElapsedTime / 60f % 60);
        hours = (int)(currentElapsedTime / 3600f % 24);
        days = (int)(currentElapsedTime / 86400f % 31);
        months = (int)(currentElapsedTime / 2.628e+6f % 12);
        years = (int)(currentElapsedTime / 3.154e+7f);

        //or (it stops IDK why)

        //seconds = (int)(currentElapsedTime % 60);
        //minutes = (int)(currentElapsedTime / 60f % 60);
        //hours = (int)(currentElapsedTime / 3600f % 24);
        //days = (int)(currentElapsedTime / 86400f % 31);
        //months = (int)(currentElapsedTime / 2.628e+6f % 12);
        //years = (int)(currentElapsedTime / 3.154e+7f);
    }

    private void WriteTimes()
    {
        timeSlots[0].text = seconds.ToString("00");
        timeSlots[1].text = minutes.ToString("00");
        timeSlots[2].text = hours.ToString("00");
        timeSlots[3].text = days.ToString("00");
        timeSlots[4].text = months.ToString("00");
        timeSlots[5].text = years.ToString("00");

        currentTimeScale.text = unityTimeScale.ToString() + "x";

    }

}
