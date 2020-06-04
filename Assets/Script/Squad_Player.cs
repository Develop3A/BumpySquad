using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad {


    Camera mainCamera;
    public int max_doubletap_time = 15;
    int doubletap_time = 0;

    public Vector3 m_pos;
    public Vector3 m_pos_down;


    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        doubletap_time = 0;
    }

    void Update()
    {
        if(doubletap_time >=1)doubletap_time -= 1;
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
            
            if (doubletap_time <= 0 && !isDash) doubletap_time = max_doubletap_time;
            else if (doubletap_time >0 && !isDash)
            {
                doubletap_time = 0;
                Dash(true);
                return;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
           if(100< Mathf.Abs((m_pos.x - m_pos_down.x) + (m_pos.y - m_pos_down.y)))
            {
                Turnback(true);
            }
            Set_Curving(false);
        }
    }

    public override void Curve(Vector3 vec)
    {
        rot_target.position = transform.position;
        rot_target.LookAt(vec);
        float f = 0;
        if (rot_target.localEulerAngles.y >= 180 && rot_target.localEulerAngles.y < 315)
        {
            f = -45;
        }
        else if (rot_target.localEulerAngles.y > 45 && rot_target.localEulerAngles.y < 180)
                {
            f = 45;
        } 
        else
        {
            f = 0;
        }
        rot_target.localRotation = Quaternion.Euler(0,f , 0);
        Set_Curving(true);
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
    }
}
