using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooltime_timer : MonoBehaviour {

    public int Skill_Order;
    public float cool_time;
    public float present =0.0f;
    public Text timer;

    void Awake()
    {
        if (timer == null) timer = GetComponent<Text>();

    }

    public void Start_Cooltime()
    {
        present = cool_time;
        StartCoroutine("Active");
    }

    IEnumerator Active()
    {
        float pre_time = Time.time;
        GetComponent<Button>().interactable = true;
        while (present > 0)
        {
            present = cool_time - (Mathf.Round((Time.time - pre_time )* 10.0f) / 10f);
            Mathf.Clamp(present, 0, cool_time);
            timer.text = present.ToString();
            yield return new WaitForEndOfFrame();
        }
    }

}
