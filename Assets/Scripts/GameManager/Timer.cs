using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private TMP_Text timerText;
    enum TimerType {Countdown, Stopwatch}
    [SerializeField] private TimerType timerType;
    [SerializeField] private float timeToDisplay = 00.0f;
    [SerializeField] private GameObject instructions;

    private bool isRunning;
    private void Awake()
    {
        timerText = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
        EventManager.TimerUpdate += EventManagerOnTimerUpdate;
    }
    private void OnDisable()
    {
        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
        EventManager.TimerUpdate -= EventManagerOnTimerUpdate;
    }
    private void EventManagerOnTimerStart() => isRunning = true;
    private void EventManagerOnTimerStop() => isRunning = false;
    private void EventManagerOnTimerUpdate(float value) => timeToDisplay += value;
    // Update is called once per frame
    void Update()
    {
        
        if (!isRunning) return;
        if (timerType == TimerType.Countdown && timeToDisplay < 0.0f) return;
        if (timeToDisplay > 20.0f)
        {
            DeactivateInstructions(); 
        }
        timeToDisplay += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        timerText.text = timeSpan.ToString(@"mm\:ss\:ff");
    }


    void DeactivateInstructions()
    {
        instructions.SetActive(false); 
    }
}
