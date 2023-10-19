using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] TMP_Text countdownField;
    [SerializeField] Timer timerScript;
    [SerializeField] GameObject arrow; 
    [SerializeField] GameObject arrowText;
    [SerializeField] GameObject player; 

    float timeScaleOnPause = 1f;
    float timer = 4f;
    bool started;
    bool finished;

    void Update()
    {
        if (started)
        {
            //Debug.Log($"Finished: {finished}, Started: {started}, Timer: {Mathf.Floor(timer)}");
            if (finished)
            {
                return;
            }
            if (timer > 1)
            {
                timer -= Time.unscaledDeltaTime;
                calculateTimerValue(timer);
            }
            else
            {
                timerScript.StartTimer();
                finished = true;
                started = false;
                countdownField.text = "START!";
                Time.timeScale = timeScaleOnPause;
                player.GetComponent<Controls>().enabled = true;
            }
        }
    }

    public void StartCountdown()
    {
        GetComponent<Animator>().enabled = true;
        gameObject.SetActive(true);
        arrow.SetActive(false);
        arrowText.SetActive(false);
        player.GetComponent<PlayerLook>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
        RuntimeManager.MuteAllEvents(false);
        Time.timeScale = 0f;
        started = true;
        finished = false;
    }


    public void ResetCountdown()
    {
        started = true;
        finished = false;
        timer = 4f;
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Swirl");
        player.GetComponent<Controls>().enabled = false;
        this.timeScaleOnPause = 1;
    }   

    public void Disable()
    {
        finished = true;
        started = false;
    }

    public void Cancel()
    {
        started = false;
        finished = false;
        GetComponent<Animator>().enabled = false;
    }

    private void calculateTimerValue(float timer)
    {
        TimeSpan time = TimeSpan.FromSeconds(timer);
        countdownField.text = time.ToString("%s");
    }

    public void ClearText()
    {
        countdownField.text = "";
    }

    public void FinishText()
    {

        countdownField.text = "FINISH!";
        GetComponent<Animator>().Play("Finish");
    }
}
