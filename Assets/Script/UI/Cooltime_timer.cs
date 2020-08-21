using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooltime_timer : MonoBehaviour {

    public int Skill_Order;
    public Text timer;

    void Awake()
    {
        if (timer == null) timer = GetComponent<Text>();

    }

    public void Set_Cooltime(float remain_cooltime)
    {
        remain_cooltime = Mathf.Round(remain_cooltime * 10.0f) / 10.0f;
        timer.text = remain_cooltime.ToString();
    }
    

}
