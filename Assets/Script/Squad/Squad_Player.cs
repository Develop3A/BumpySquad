using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad {


    [Space(15)]
    [Header("Player Option")]
    [Header("스킬 옵션")]
    public float Dash_cooltime;
    public float Dash_duration;
    public float Dash_knockback_time;
    public float Dash_knockback_speed;
    public float Dash_sturn_duration;
    bool can_dash;
    public float Turnback_cooltime;
    bool can_turnback;

    [Space(20)]
    [Header("각도")]
    public bool isAbs;
    public float rotate_force;
    public float maxangular;
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
        GetComponent<Rigidbody>().maxAngularVelocity = maxangular;
        int f = Application.targetFrameRate;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rotation_speed = rotate_force / f;
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
            if(!IsInvoking("Set_Curve_delay_Off") & isCurve_delay) Invoke("Set_Curve_delay_Off", curve_delay_time);
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
        if (isAbs)
        {
            if (rot_target.eulerAngles.y >= 180 && rot_target.eulerAngles.y < 359)
            {
                curve_isright = false;
            }
            else if (rot_target.eulerAngles.y > 0 && rot_target.eulerAngles.y < 180)
            {
                curve_isright = true;
            }
        }
        else
        {
            if (rot_target.localEulerAngles.y >= 180 && rot_target.localEulerAngles.y < 359)
            {
                curve_isright = false;
            }
            else if (rot_target.localEulerAngles.y > 0 && rot_target.localEulerAngles.y < 180)
            {
                curve_isright = true;
            }
        }
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

    public void Dash_FrontLine(Soldier s,Collision c)
    {
        bool end = false;
        for(int i = 0; i<soldiers.Length; i++)
        {
            if(soldiers[i] == s)
            {
                if (end) break;
                if (i == 0 || i == 1 || i == 2)
                {
                    if (soldiers[0] != null || soldiers[1] != null || soldiers[2] != null)
                    {
                            if (isDash)
                            {
                                c.gameObject.GetComponent<Squad>().Knockback(true, Dash_knockback_time, Dash_knockback_speed, Dash_sturn_duration);

                                Dash_Off();
                                speed = 0;
                            }
                            isColliderContact = true;
                        end = true;
                    }
                }
                else if(i == 3 || i == 4 || i == 5)
                {
                    if (soldiers[3] != null || soldiers[4] != null || soldiers[5] != null)
                    {
                            if (isDash)
                            {
                                c.gameObject.GetComponent<Squad>().Knockback(true, Dash_knockback_time, Dash_knockback_speed, Dash_sturn_duration);

                                Dash_Off();
                                speed = 0;
                            }
                            isColliderContact = true;
                        end = true;
                    }
                }
                else if (i == 6 || i == 7 || i == 8)
                {
                    if (soldiers[6] != null || soldiers[7] != null || soldiers[8] != null)
                    {
                            if (isDash)
                            {
                                c.gameObject.GetComponent<Squad>().Knockback(true, Dash_knockback_time, Dash_knockback_speed, Dash_sturn_duration);

                                Dash_Off();
                                speed = 0;
                            }
                            isColliderContact = true;
                        end = true;
                    }
                }
            }
        }
    }

    IEnumerator Use_Dash()
    {
        if(can_dash)
        {
            can_dash = false;
            Dash(true, Dash_duration);
            GameManager.gm.Start_Cooltime_tiemr(0);
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
            GameManager.gm.Start_Cooltime_tiemr(1);
            yield return new WaitForSeconds(Turnback_cooltime);

            can_turnback = true;
        }
    }

    void OnCollisionStay(Collision c)
    {
        if (!isEnemy)
        {
            if (c.gameObject.tag == "Enemy")
            {
                if (isDash)
                {
                    c.gameObject.GetComponent<Squad>().Knockback(true, Dash_knockback_time, Dash_knockback_speed,Dash_sturn_duration);

                    Dash_Off();
                    speed = 0;
                }
                isColliderContact = true;
            }
        }
        else
        {
            if (c.gameObject.tag == "Player")
            {
                if (isDash)
                {
                    c.gameObject.GetComponent<Squad>().Knockback(true);

                    Dash_Off();
                    speed = 0;
                }
                isColliderContact = true;
            }
        }

        if (isDash)
        {
            if (c.gameObject.tag == "rock")
            {
                Debug.Log("contact rock");
                Dash_Off();
            }
        }
    }

}
