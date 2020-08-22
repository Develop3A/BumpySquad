using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : MonoBehaviour {

    public int Skill_Number;
    public int Skill_Order;
    protected Squad squad;
    public float cooltime;
    bool can_use = false;

    void Awake()
    {
        First();
    }

    protected virtual void First()
    {
        squad = GetComponent<Squad>();

        StartCoroutine("Cooltimer");
    }

    public virtual void Use()
    {
        if (can_use)
        {
            can_use = false;
            StartCoroutine("Cooltimer");
        }
        else
        {
            Debug.Log("a");
            return;
        }
    }

    public IEnumerator Cooltimer()
    {
        float present_time = Time.time;

        while (Time.time - present_time < cooltime)
        {
            PlayerManager.pm.Set_Cooltime_timer(PlayerManager.pm.timers[Skill_Order],-( Time.time - present_time - cooltime) );
            yield return new WaitForEndOfFrame();
        }
        PlayerManager.pm.Set_Cooltime_timer(PlayerManager.pm.timers[Skill_Order], 0.0f);
        can_use = true;
    }

}
