using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad {


    [Space(15)]
    [Header("Player Option")]
    [Header("각도")]
    public bool isAbs;
    public float rotate_force;
    protected float maxangular =2;
    Camera mainCamera;


    
    
    public override void Ready()
    {
        int f = Application.targetFrameRate;
        base.Ready();
        accel = accel_ / f;
        GetComponent<Rigidbody>().maxAngularVelocity = maxangular;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        rotation_speed = rotate_force / f;
        Set_Curve_delay_On();
    }

    public override void Set_Active(bool value)
    {
        if (value)
        {
            isActive = true;
            StartCoroutine("Active");
        }
        else
        {
            isActive = false;
        }
    }

    protected override IEnumerator Active()
    {
        while (isActive)
        {
            Sum_speed(accel);
            float yv = rigid.velocity.y;
            Vector3 vel = transform.forward * speed * Application.targetFrameRate * Time.deltaTime;
            rigid.velocity = new Vector3(vel.x, yv, vel.z);

            if (isCurving)
            {
                if (curve_isright && !isCurve_delay) transform.Rotate(Vector3.up * rotation_speed);
                else if (!isCurve_delay) transform.Rotate(Vector3.up * -rotation_speed);
            }
            Contact_Check();

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
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
