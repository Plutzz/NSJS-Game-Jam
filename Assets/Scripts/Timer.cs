using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    [Header("Time Varibles (Seconds)")]
    [Tooltip("How long between each curse")]
    [SerializeField] private float timePerCurse;
    private float timeUntilNextCurse;

    public static Timer timer;

    public bool TimerOn = false;

    private float currentTime;      //time in seconds

    public TextMeshProUGUI TimerTxt;

    private void Awake()
    {
        timer = this;
    }

    void Start()
    {
        TimerOn = true;
        timeUntilNextCurse = timePerCurse;
    }

    void Update()
    {
        // Checks if time is up and stops the timer if it has
        if (TimerOn)
        {
            if (timeUntilNextCurse > 0)
            {
                timeUntilNextCurse -= Time.deltaTime;
                currentTime += Time.deltaTime;
                UpdateTimer(currentTime);
            }
            else
            {
                CurseTime();
                timeUntilNextCurse = timePerCurse;
            }
        }
    }

    // Updates the timer text
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public string GetTime()
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public event Action onCurseTime;
    public void CurseTime()
    {
        if(onCurseTime != null)
        {
            onCurseTime();
        }
    }

}
