﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : Squad_property
{
    float accel;
    //float curve_decel;
    
    Vector3 box_pos;
    Vector3 box_size;
    Vector3 knockback_dir;

    #region 보이지 않는 값
    [Space(10)]
    public float speed;
    protected float knockback_time;
    protected float max_knockback_speed_persecond;
    protected float rotation_speed = 0.025f;
    protected bool curve_isright;
    protected bool isEnemy;
    protected bool isContact;
    protected bool isColliderContact;
    protected bool isSturn;
    bool isMire;
    bool isActive;
    bool isCurving;
    bool isKnockback;
    public Rigidbody rigid { get; set; }
    protected Soldier[] soldiers = new Soldier[9];
    public Skill[] skills;
    #endregion

    public virtual void Ready()
    {
        rigid = GetComponent<Rigidbody>();
        speed = 0;
        accel = accel_ ;
        //curve_decel = curve_decel_ ;

        box_size = new Vector3(yellowboxsize, yellowboxsize, yellowboxsize + plus_z);
    }

    public void Set_soldier_num(GameObject soldier_obj,int num)
    {
        soldiers[num] = soldier_obj.GetComponent<Soldier>();
        soldier_obj.GetComponent<Soldier>().Squad = this;
    }
    public Soldier[] Get_soldier()
    {
        return soldiers;
    }

    public void Sum_speed(float value)
    {
        if(isKnockback)
        {
            isCurving = false;
            if (!IsInvoking("Knockback_Off"))
            {
                Invoke("Knockback_Off", knockback_time);
            }
            speed = max_knockback_speed_persecond;
        }
        else if(isCurving )
        {
            //if (speed > max_curve_speed_persecond) speed -= curve_decel;
            //else 
            speed += accel;
            if(isMire) speed = Mathf.Clamp(speed, 0, Clamp_Mire_speed(max_straight_speed_persecond));
            else speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond); ;
        }
        else if(isContact& isColliderContact)
        {
            if (speed > max_contact_speed_persecond) speed = max_contact_speed_persecond;
            else speed += accel;
            speed = Mathf.Clamp(speed, 0, max_contact_speed_persecond);
        }
        else
        {
            speed += value;
            if (isMire) speed = Mathf.Clamp(speed, 0, Clamp_Mire_speed(max_straight_speed_persecond));
            else speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond); ;
        }
    }
    public void Set_speed(float value)
    {
        speed = value;
        speed = Mathf.Clamp(speed, speed, value);
    }

    public void Set_Active(bool value)
    {
        if(value)
        {
            isActive = true;
            StartCoroutine("Active");
        }
        else
        {
            isActive = false;
        }
    }
    public void Set_Curving(bool value)
    {
        isCurving = value;
        if(!isEnemy)
        {
            if (value) rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            else rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    public virtual void Curve(Vector3 vec)
    {
        
    }
    public void Contact_Check()
    {
        box_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + plus_z);
        Collider[] colliders = Physics.OverlapBox(box_pos, box_size * 0.5f, transform.rotation);

        bool contact = false;

        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (!isEnemy & s.isEnemy)
                {
                    contact = true;
                    break;
                }
                else if (isEnemy & !s.isEnemy)
                {
                    contact = true;
                    break;
                }
            }
            catch
            {
                continue;
            }

            isContact = contact;
            if (!isContact) isColliderContact = false;
        }
    }
    public void Contact_check(out List<Squad> ss)
    {
        //뒤로 돌아스킬을 사용할때 접촉한 적이 있는지 판단하고 넉백을 주기위한 함수임.
        box_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + plus_z);
        Collider[] colliders = Physics.OverlapBox(box_pos, box_size * 0.5f, transform.rotation);

        bool contact = false;
        ss = new List<Squad>();
        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (!isEnemy & s.isEnemy)
                {
                    contact = true;
                    //s가 적 용병일경우 
                    if (ss.Count != 0)
                    {
                        bool dupli = false;
                        foreach (Squad squad in ss)
                        {
                            if (s.Squad != squad) continue;
                            else dupli = true;
                        }

                        if(!dupli)
                        {
                            ss.Add(s.Squad);
                        }
                    }
                    else
                    {
                        ss.Add(s.Squad);
                    }
                    break;
                }
                else if (isEnemy & !s.isEnemy)
                {
                    contact = true;
                    break;
                }

            }
            catch
            {
                continue;
            }

            isContact = contact;
            if (!isContact) isColliderContact = false;
        }
    }
    #region 플레이어의 특수상태나 스킬

    public void Knockback(bool value, float time, float knockback_speed,float sturn_time,Transform player)
    {
        if(!value)
        {
            if (!IsInvoking("Knockback_Off")) Invoke("Knockback_Off", time);
        }
        if (isKnockback) return;
        else if(value)
        {
            /*
            if (sturn_time == 0)
            {
                GameObject g = new GameObject();
                g.transform.position = transform.position;
                g.transform.LookAt(player);
                knockback_dir = g.transform.rotation.eulerAngles;
            }
            else
            {
            }
            */
            knockback_dir = player.forward;
            isKnockback = true;
            max_knockback_speed_persecond = knockback_speed;
            knockback_time = time;
            sturn_duration = sturn_time;
            Invoke("Knockback_Off", time);
        }
    }
    public void Knockback_Off()
    {
        isKnockback = false;
    }

    public void Sturn()
    {
        Sturn(true);
    }
    public void Sturn(bool value)
    {
        if (!value)
        {
            isSturn = false;
        }

        if (isSturn) return;
        else if (value)
        {
            isSturn = true;
            Sturn_soldiers();
        }
    }
    public void Sturn_soldiers()
    {
        for(int i = 0; i<soldiers.Length; i++)
        {
            if (soldiers[i] == null) continue;
            else
            {
                soldiers[i].Set_Fighting(false);
                soldiers[i].soldier_sturn_time = sturn_duration;
                soldiers[i].StartCoroutine("Soldier_Sturn");
            }
        }
    }
    

    public void Set_Mire(bool value)
    {
        isMire = value;
    }
    float Clamp_Mire_speed(float f)
    {
        return f * max_mire_speed_ratio;
    }
#endregion

    protected void Set_Curve_delay_On()
    {
        isCurve_delay = true;
        CancelInvoke("Set_Curve_delay_Off");
    }
    protected void Set_Curve_delay_Off()
    {
        isCurve_delay = false;
    }

    IEnumerator Active()
    {
        while (isActive)
        {
            Sum_speed(accel);
            if (isKnockback)
                rigid.velocity = knockback_dir * -speed * Application.targetFrameRate * Time.deltaTime;
            else
                rigid.velocity = transform.forward * speed * Application.targetFrameRate * Time.deltaTime;

            if (isSturn)
            {
                yield return new WaitForSeconds(sturn_duration);
                speed = 0.0f;
                Sturn(false);
                Knockback_Off();
            }
            else if (isCurving)
            {
                if (isEnemy)
                {
                    Quaternion q = Quaternion.Slerp(transform.rotation, rot_target.rotation, rotation_speed );
                    Quaternion ori_q = transform.rotation;
                    transform.rotation = q;

                    if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(ori_q.eulerAngles.y)) < 0.1f)
                        isCurving = false;
                }
                else
                {
                    if (curve_isright &&!isCurve_delay) transform.Rotate(Vector3.up * rotation_speed);
                    else if(!isCurve_delay) transform.Rotate(Vector3.up *-rotation_speed);
                }
            }
            Contact_Check();

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }



    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero+ new Vector3(0,0,plus_z),box_size);
    }
}
