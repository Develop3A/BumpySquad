using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad
{


    [Space(15)]
    [Header("Player Option")]
    [Header("각도")]
    [HideInInspector]public bool isAbs;
    protected float maxangular = 2;
    Camera mainCamera;

    public override void Ready()
    {
        int f = Application.targetFrameRate;
        base.Ready();
        GetComponent<Rigidbody>().maxAngularVelocity = maxangular;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Set_Curve_delay_On();
    }

    public override void Set_Active(bool value)
    {
        if (value)
        {
            isActive = true;
            StartCoroutine("Active");
            //Debug.Log(gameObject.name + " Active");
        }
        else
        {
            isActive = false;
        }
    }

    /*
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
    */

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsInvoking("Set_Curve_delay_Off") & isCurve_delay) Invoke("Set_Curve_delay_Off", curve_delay_time);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                Curve(hit.point);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Set_Curve_delay_On();
        }
        if (Input.GetMouseButtonDown(1))
        {

            rigid.velocity = -rigid.velocity;
        }
    }

    public override void Curve(Vector3 vec)
    {
        rot_target.position = transform.position;
        rot_target.LookAt(vec);

        //누르는 위치를 계산해서 분대의 전진방향
        if (rot_target.eulerAngles.y >= 0 && rot_target.eulerAngles.y < 45
            || rot_target.eulerAngles.y >= 315 && rot_target.eulerAngles.y < 361)
        {
            Set_Direction(Direction.front);
        }
        else if (rot_target.eulerAngles.y >= 45 && rot_target.eulerAngles.y < 135)
        {
            Set_Direction(Direction.right);
        }
        else if (rot_target.eulerAngles.y >= 135 && rot_target.eulerAngles.y < 225)
        {
            Set_Direction(Direction.back);
        }
        else if (rot_target.eulerAngles.y >= 225 && rot_target.eulerAngles.y < 315)
        {
            Set_Direction(Direction.left);
        }

        //누르는 위치 계산해서 분대기준 왼쪽인지 오른쪽인지
        if (rot_target.localEulerAngles.y >= 180 && rot_target.localEulerAngles.y < 359)
        {
            curve_isright = false;
        }
        else if (rot_target.localEulerAngles.y > 0 && rot_target.localEulerAngles.y < 180)
        {
            curve_isright = true;
        }
    }

    protected override void Set_Curve_delay_Off()
    {
        //진영을 회전시키는것은 커브딜레이가 꺼질때에 쓸거기때문에 오버라이딩
        base.Set_Curve_delay_Off();
        StartCoroutine("Rotate_Squad");
    }
    IEnumerator Rotate_Squad()//진영을 회전시킬때
    {
        Set_Curving(true);
        //Debug.Log("Start Rotate_squad");
        Quaternion target_rot = Quaternion.identity;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 360, transform.eulerAngles.z);

        if (curve_isright)
            target_rot = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
        else
            target_rot = Quaternion.Euler(0, transform.eulerAngles.y - 90, 0);

        Set_speed(speed - rotateDecelSpeed);
        float max_time = rotateDecelTime ;
        for (float f = 0; f<max_time; f +=Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target_rot, f / max_time);
            if(f / rotateDecelTime > 0.5f && isCurving)
            {
                Set_Curving(false);
            }
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = target_rot;
        yield return null;
    }

    public override void Bounce_byObject(Vector3 contactPoint)
    {
        base.Bounce_byObject(contactPoint);
    }


}