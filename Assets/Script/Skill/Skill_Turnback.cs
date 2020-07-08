using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Turnback : Skill {

    bool can_turnback;
    bool can_swipe = false;
    float turnback_knockback_time;
    float turnback_knockback_speed;
    public float swipe_distance;
    public float swipe_time;
    public Vector3 m_pos;
    public Vector3 m_pos_down;

    protected override void First()
    {
        base.First();

        //임시로 여기에 붙임
        Skill_Number = 1;
        turnback_knockback_time = 0.1f;
        turnback_knockback_speed = -30.0f;

        cooltime = 10;
        swipe_distance = 500;
        swipe_time = 0.2f;

        can_turnback = true;
    }

    #region 스킬 입력관련 부분
    void Update()
    {
        m_pos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            //클릭하면
            m_pos_down = Input.mousePosition; //마우스의 클릭된 좌표가 저장되고
            if (!can_swipe)
            {
                //스와이프가 가능한상태라면
                Can_Swipe_On(); //일정시간동안 안에 마우스를 뗐을때 조건이 성립할경우 (일정 거리 이상일경우) 스와이프를 실행함.
                Invoke("Can_Swipe_Off", swipe_time);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float x = (m_pos.x - m_pos_down.x) * (m_pos.x - m_pos_down.x);
            float y = (m_pos.y - m_pos_down.y) * (m_pos.y - m_pos_down.y);
            float dis = Mathf.Sqrt(x + y);
            if (swipe_distance < Mathf.Abs(dis) && can_swipe)
            {
                Can_Swipe_Off();
                StartCoroutine("Use_Turnback");
            }
        }
    }
    public override void Use()
    {
        StartCoroutine("Use_Turnback");
    }

    void Can_Swipe_On()
    {
        can_swipe = true;
    }
    void Can_Swipe_Off()
    {
        can_swipe = false;
        CancelInvoke("Can_Swipe_Off");
    }
    IEnumerator Use_Turnback()
    {
        if (can_turnback)
        {
            can_turnback = false;
            #region 스킬의 내용
            List<Squad> contact_squads;
            squad.Contact_check(out contact_squads);
            foreach (Squad s in contact_squads)
            {
                s.Set_Knockback(true, turnback_knockback_time, turnback_knockback_speed, 0, transform);
            }
            transform.Rotate(0, 180, 0);
            #endregion
            yield return new WaitForSeconds(cooltime);

            can_turnback = true;
        }
    }
    #endregion
}
