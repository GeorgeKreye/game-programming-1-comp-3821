using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Timer Configuration")]
    [Tooltip("Length that this timer will run")]
    [SerializeField] private float timerDuration;

    // fields we need to handle the timer
    private bool timerStarted;
    private float endTime;

    // each of these will take a function that takes a float as the argument, how much time has elapsed 
    [Header("Timer Events")]
    [SerializeField] private UnityEvent<Timer> OnTimerStarted;
    [SerializeField] private UnityEvent<Timer> OnTimerExpired;
    [SerializeField] private UnityEvent<Timer> OnTimerTick;

    // returns true if the timer has started
    public bool TimerStarted { get { return timerStarted; } }

    /// <summary>
    /// Gets or sets the duration of the timer. Should not be set while the timer is running.
    /// </summary>
    public float Duration { get { return timerDuration; } set { timerDuration = value; } }

    /// <summary>
    /// Returns how much time is left on this timer.
    /// </summary>
    /// How much time is left on the timer
    public float TimeLeft()
    {
        float left = endTime - Time.time;
        return (left <= 0.0f ? 0.0f : left);
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        if (!TimerStarted)
        {
            timerStarted = true;
            StartCoroutine(StartCountdown());
        }
    }

    /// <summary>
    /// Resets the timer so that next time Start() is called, it will run the full Duration
    /// </summary>
    public void ResetTimer()
    {
        timerStarted = false;
    }

    protected virtual IEnumerator StartCountdown()
    {
        // calculate our end time
        endTime = Time.time + Duration;
        OnTimerStarted.Invoke(this);

        // now loop until the current elapsed time is greater than the end time
        while (Time.time < endTime)
        {
            // now send a tick message message
            OnTimerTick.Invoke(this);
            yield return null;
        }

        // now fire the last message, that the timer has finished
        OnTimerExpired.Invoke(this);
    }
}