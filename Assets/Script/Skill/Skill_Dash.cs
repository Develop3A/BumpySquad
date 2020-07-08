using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Dash : Skill
{
    bool can_dash;
    public float max_dash_speed_persecond;
    protected bool isDash;
    public float Dash_duration;
    public float Dash_knockback_time;
    public float Dash_knockback_speed;
    public float Dash_sturn_duration;

    [Space(20)]
    bool can_doubletap = false;
    public float doubletap_time = 0.5f;

    protected override void First()
    {
        base.First();

        Skill_Number = 0;
        max_dash_speed_persecond = 15;
        cooltime = 5;
        Dash_duration = 0.25f;
        Dash_knockback_time = 0.15f;
        Dash_knockback_speed = -30;
        Dash_sturn_duration = 3;
        can_dash = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!can_doubletap && !isDash)
            {
                Can_Doubletap_On();
                Invoke("Can_Doubletap_Off", doubletap_time);
            }
            else if (can_doubletap && !isDash)
            {
                Can_Doubletap_Off();
                Use();
                return;
            }
        }
    }

    void Can_Doubletap_On()
    {
        can_doubletap = true;
    }
    void Can_Doubletap_Off()
    {
        can_doubletap = false;
        CancelInvoke("Can_Doubletap_Off");
    }

    public override void Use()
    {
        if (isDash) return;
        else
        {
            isDash = true;
            PlayerManager.pm.Start_Cooltime_timer(Skill_Order);
            StartCoroutine("Use_Dash");
        }
    }
    IEnumerator Use_Dash()
    {
        float present_time = Time.time;
        if (can_dash)
        {
            can_dash = false;
            #region 스킬 내용
            squad.Set_Curving(false);
            //Debug.Log(Time.time + "   " +(present_time + Dash_duration));
            while (Time.time < present_time + Dash_duration && isDash)
            {
                //Debug.Log("Dash");
                squad.Set_speed(max_dash_speed_persecond);
                squad.rigid.velocity = transform.forward * squad.speed * Application.targetFrameRate * Time.deltaTime;
                Check_FrontLine();
                yield return new WaitForEndOfFrame();
            }
            isDash = false;
            #endregion
            yield return new WaitForSeconds(cooltime);
            isDash = false;
            can_dash = true;
        }

        yield return new WaitForEndOfFrame();
    }
    public void Check_FrontLine()
    {
        bool end = false;
        Soldier[] soldiers = squad.Get_soldier();
        for (int i = 0; i < soldiers.Length; i++)
        {
            GameObject target = null;
            if (soldiers[i] == null) continue;
            Detect_FrontLine(soldiers[i],out target);
            if (target != null)
            {
                if (i == 0 || i == 1 || i == 2)
                {
                    if (soldiers[0] != null | soldiers[1] != null | soldiers[2] != null)
                    {
                        //Debug.Log(i);
                        if (target.tag == "Enemy")
                        {
                            target.GetComponent<Squad>().Set_Knockback(true, Dash_knockback_time, Dash_knockback_speed, Dash_sturn_duration, transform);
                            if (target.GetComponent<Squad>().Squad_Type == Squad_property.Squad_Category.Solo_squad)
                            {
                                continue;
                            }
                        }
                        isDash = false;
                        squad.Set_speed(0);
                        end = true;
                    }
                }
                else if ((i == 3 || i == 4 || i == 5) && !end)
                {
                    if (soldiers[3] != null || soldiers[4] != null || soldiers[5] != null)
                    {
                        Debug.Log(i);
                        if (target.tag == "Enemy")
                        {
                            target.GetComponent<Squad>().Set_Knockback(true, Dash_knockback_time, Dash_knockback_speed, Dash_sturn_duration, transform);
                            if (target.GetComponent<Squad>().Squad_Type == Squad_property.Squad_Category.Solo_squad)
                            {
                                continue;
                            }
                        }
                        isDash = false;
                        squad.Set_speed(0);
                        end = true;
                    }
                }
                else if ((i == 6 || i == 7 || i == 8) && !end)
                {
                    if (soldiers[6] != null || soldiers[7] != null || soldiers[8] != null)
                    {
                        Debug.Log(i);
                        if (target.tag == "Enemy")
                        {
                            target.GetComponent<Squad>().Set_Knockback(true, Dash_knockback_time, Dash_knockback_speed, Dash_sturn_duration, transform);
                            if (target.GetComponent<Squad>().Squad_Type == Squad_property.Squad_Category.Solo_squad)
                            {
                                continue;
                            }
                        }
                        isDash = false;
                        squad.Set_speed(0);
                        end = true;
                    }
                }
            }
            if (end) break;
        }
    }
    public void Detect_FrontLine(Soldier soldier,out GameObject target)
    {
        target = null;
        Collider[] colls = Physics.OverlapSphere(soldier.transform.position, 0.55f);

        foreach (Collider c in colls)
        {
            try
            {
                //상대가 적인지 먼저 가늠함
                Soldier s = c.transform.GetComponent<Soldier>();
                if (!s.isEnemy) continue;
                else if (s.isEnemy)
                {
                    target = s.Squad.gameObject;
                    break;
                }
            }
            catch
            {
                //아니라면 바위인지 체크
                if (c.gameObject.tag == "rock")
                {
                    squad.Set_speed(0);
                    isDash = false;
                }
                continue;
            }
        }
    }


}
