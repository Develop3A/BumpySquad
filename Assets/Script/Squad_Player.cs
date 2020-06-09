using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad {


    [Space(15)]
    [Header("Player Option")]
    [Header("스킬 - 쿨타임")]
    public float Dash_cooltime;
    bool can_dash;
    public float Turnback_cooltime;
    bool can_turnback;

    [Space(20)]
    [Header("각도")]
    public float rotate_persecond;
    Camera mainCamera;

    [Space(20)]
    bool can_doubletap = false;
    public float doubletap_time = 0.5f;

    bool can_swipe = false;
    public float swipe_distance;
    public float swipe_time;
    public Vector3 m_pos;
    public Vector3 m_pos_down;
    
    void Start()
    {
        int f = Application.targetFrameRate;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rotation_speed = rotate_persecond / f;
        Set_Curve_delay_On();
        Set_Active(true);
        can_dash = true;
        can_turnback = true;
    }

    void Update()
    {
        m_pos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            m_pos_down = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                Curve(hit.point);
            }

            if (!can_doubletap && !isDash)
            {
                Can_Doubletap_On();
                Invoke("Can_Doubletap_Off",doubletap_time);
            }
            else if (can_doubletap && !isDash)
            {
                Can_Doubletap_Off();
                StartCoroutine("Use_Dash");
                return;
            }

            if(!can_swipe)
            {
                Can_Swipe_On();
                Invoke("Can_Swipe_Off",swipe_time);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            float x = (m_pos.x - m_pos_down.x) * (m_pos.x - m_pos_down.x);
            float y = (m_pos.y - m_pos_down.y) * (m_pos.y - m_pos_down.y);
            float dis = Mathf.Sqrt(x + y);
            if (swipe_distance < Mathf.Abs(dis) && can_swipe)
            {
                Can_Swipe_Off();
                StartCoroutine("Use_Turnback");
            }

            Set_Curve_delay_On();
            Set_Curving(false);
        }
    }

    public override void Curve(Vector3 vec)
    {
        rot_target.position = transform.position;
        rot_target.LookAt(vec);
        if (rot_target.localEulerAngles.y >= 180 && rot_target.localEulerAngles.y < 359)
        {
            curve_isright = false;
        }
        else if (rot_target.localEulerAngles.y > 0 && rot_target.localEulerAngles.y < 180)
                {
            curve_isright = true;
        }
        Invoke("Set_Curve_delay_Off", curve_delay_time);
        Set_Curving(true);
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
    void Can_Swipe_On()
    {
        can_swipe = true;
    }
    void Can_Swipe_Off()
    {
        can_swipe = false;
        CancelInvoke("Can_Swipe_Off");
    }

    IEnumerator Use_Dash()
    {
        if(can_dash)
        {
            can_dash = false;
            Dash(true);
            yield return new WaitForSeconds(Dash_cooltime);

            can_dash = true;
        }
    }
    IEnumerator Use_Turnback()
    {
        if(can_turnback)
        {
            can_turnback = false;
            Turnback(true);
            yield return new WaitForSeconds(Turnback_cooltime);

            can_turnback = true;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (isDash&  c.gameObject.tag == "Enemy")
        {
            Squad s = c.gameObject.GetComponent<Squad>();
            s.Knockback(true);

            Dash(false);
            speed = 0;
        }
        if (c.gameObject.tag == "Enemy")
        {
            isContact = true;
        }
    }
    void OnCollisionStay(Collision c)
    {
        if (isDash & c.gameObject.tag == "Enemy")
        {
            Squad s = c.gameObject.GetComponent<Squad>();
            s.Knockback(true);

            Dash(false);
            speed = 0;
        }
    }
    void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            isContact = false;
        }
    }
}
