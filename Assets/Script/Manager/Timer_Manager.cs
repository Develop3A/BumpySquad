using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_Manager : MonoBehaviour {

    public bool ReversTimer;
    public float limit_time;
    public Text timer;


    void Update()
    {
        if (ReversTimer)
        {
            timer.text = limit_time - Mathf.Round(Time.time * 10.0f) / 10f + "";
        }
        else
        {
            timer.text = Mathf.Round(Time.time * 10.0f) / 10f + "";
        }
    }
}
