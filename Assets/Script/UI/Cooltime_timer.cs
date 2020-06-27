using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooltime_timer : MonoBehaviour {

    public string skillname;
    public float limit_time;
    float present =0.0f;
    public Text timer;

    void Awake()
    {
        Squad_Player sp = GameObject.FindObjectOfType<Squad_Player>();
        if (timer == null) timer = GetComponent<Text>();
        if (skillname == "Dash") limit_time =  sp.Dash_cooltime;
        else if (skillname == "Turnback") limit_time = sp.Turnback_cooltime;

    }

    public void Start_Cooltime()
    {
        present = limit_time;
        StartCoroutine("Active");
    }

    IEnumerator Active()
    {
        float pre_time = Time.time;
        while (present > 0)
        {
            present = limit_time - (Mathf.Round((Time.time - pre_time )* 10.0f) / 10f);
            Mathf.Clamp(present, 0, limit_time);
            timer.text = present.ToString();
            yield return new WaitForEndOfFrame();
        }
    }

}
