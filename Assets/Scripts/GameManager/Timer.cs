using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using LeaderboardCreatorDemo;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public TMP_Text timerText;
    public float Recordtime;
    public string RecordTimeInString;
    enum TimerType { Countdown, Stopwatch }
    [SerializeField] private TimerType timerType;
    [SerializeField] private float timeToDisplay = 00.0f;
    [SerializeField] private GameObject instructions;

    private bool isRunning;
    private void Awake()
    {
        Instance = this;
        timerText = GetComponent<TMP_Text>();
        PlayerPrefs.GetFloat("Recordtime", Recordtime);
        PlayerPrefs.GetString("RecordtimeInString", RecordTimeInString);
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
    private void EventManagerOnTimerStop()
    {
        if (timeToDisplay > Recordtime)
        {
            RecordTimeInString = timerText.text;
            Recordtime = timeToDisplay;
            PlayerPrefs.SetFloat("Recordtime", Recordtime);
            PlayerPrefs.SetString("RecordtimeInString", RecordTimeInString);
            PlayerPrefs.Save();
            LeaderboardManager.Instance.UpdateScore();
        }
        isRunning = false;
    }
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
