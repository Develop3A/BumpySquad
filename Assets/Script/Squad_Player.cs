using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad {


    [Space(15)]
    [Header("Player Option")]
    [Header("각도")]
    public float rotate_persecond;
    Camera mainCamera;

    [Space(20)]
    public int max_doubletap_time = 15;
    int doubletap_time = 0;

    public float swipe_distance;
    public Vector3 m_pos;
    public Vector3 m_pos_down;


    void Start()
    {
        int f = Application.targetFrameRate;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        doubletap_time = 0;
        rotation_speed = rotate_persecond / f;
        Set_Active(true);
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
            float x = (m_pos.x - m_pos_down.x) * (m_pos.x - m_pos_down.x);
            float y = (m_pos.y - m_pos_down.y) * (m_pos.y - m_pos_down.y);
            float dis = Mathf.Sqrt(x + y);
            Debug.Log(Mathf.Sqrt(x) + " " + Mathf.Sqrt(y) + " " + dis);
            if (swipe_distance < Mathf.Abs(dis))
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
        if (rot_target.localEulerAngles.y >= 180 && rot_target.localEulerAngles.y < 359)
        {
            curve_isright = false;
        }
        else if (rot_target.localEulerAngles.y > 0 && rot_target.localEulerAngles.y < 180)
                {
            curve_isright = true;
        } 
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
}
