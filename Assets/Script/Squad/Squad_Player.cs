using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad {


    [Space(15)]
    [Header("Player Option")]
    [Header("스킬 옵션")]
    

    [Space(20)]
    [Header("각도")]
    public bool isAbs;
    public float rotate_force;
    public float maxangular;
    Camera mainCamera;


    
    
    public override void Ready()
    {
        base.Ready();
        GetComponent<Rigidbody>().maxAngularVelocity = maxangular;
        int f = Application.targetFrameRate;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rotation_speed = rotate_force / f;
        Set_Curve_delay_On();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!IsInvoking("Set_Curve_delay_Off") & isCurve_delay) Invoke("Set_Curve_delay_Off", curve_delay_time);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                Curve(hit.point);
            }

            
        }
        if(Input.GetMouseButtonUp(0))
        {
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
    

}
