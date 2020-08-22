using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Player : Squad
{


    [Space(15)]
    [Header("Player Option")]
    public float collision_Damage;
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
    
    protected override IEnumerator Active()
    {
        while (isActive)
        {
            bool front = Front_check();
            if (isDash)
            {
                float bonus_speed = 2.0f;
                Set_speed(maxSpeed);
                SpeedBarManager.sbm.Refresh(speed / maxSpeed);
                Vector3 vel = Vector3.zero;
                vel = Get_Direction() * (speed + bonus_speed) * Time.deltaTime;
                transform.position += vel;
            }
            else
            {
                if (!front)
                {
                    float bonus_speed = 1.0f;

                    accel = Cal_accel();
                    speed = Update_speed(accel);
                    if (speed == maxSpeed)
                    {
                        collisionPower = true;
                        bonus_speed = 2.0f;
                    }
                    else collisionPower = false;
                    SpeedBarManager.sbm.Refresh(speed / maxSpeed);
                    Vector3 vel = Vector3.zero;
                    vel = Get_Direction() * (speed + bonus_speed) * Time.deltaTime;
                    transform.position += vel;
                }
                else //앞에 적이 있을경우
                {
                    if (collisionPower)
                    {
                        collisionPower = false;
                        SpeedBarManager.sbm.Refresh(speed / maxSpeed);
                        List<Squad> contact_enemies = new List<Squad>();
                        Contact_check(out contact_enemies);
                        if (contact_enemies.Count > 0)
                        {
                            foreach (Squad s in contact_enemies)
                            {
                                Soldier[] soldiers = s.Get_soldier();
                                foreach (Soldier soldier in soldiers)
                                {
                                    soldier.Sum_hp(collision_Damage);
                                }
                            }
                        }
                        else
                        {
                            //Debug.Log("None enemies!");
                        }
                        mainCamera.GetComponent<StressReceiver>().StartCoroutine("Shaking");
                        Set_speed(0);
                    }
                }
            }
            Contact_Check();

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

    void Update()
    {
        if (!isCurving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsInvoking("Set_Curve_delay_Off") & is_rotate_able) Invoke("Set_Curve_delay_Off", rotate_push_time);
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    Detect_input_rotate(hit.point);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
            {
                if (is_rotate_able)
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10000f))
                    {
                        Curve(hit.point);
                    }
                }
                Set_Curve_delay_On();
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
        
    }
    public void Detect_input_rotate(Vector3 vec)
    {
        rot_target.position = transform.position;
        rot_target.LookAt(vec);

        //누르는 위치 계산해서 분대기준 왼쪽인지 오른쪽인지
        if (rot_target.eulerAngles.y >= 181 && rot_target.eulerAngles.y < 360)
        {
            curve_isright = false;
            input_rotate = true;
        }
        else if (rot_target.eulerAngles.y >= 0 && rot_target.eulerAngles.y < 180)
        {
            curve_isright = true;
            input_rotate = true;
        }
        else
        {
            //Debug.Log("is front or back");
            input_rotate = false;
        }
    }

    protected override void Set_Curve_delay_Off()
    {
        //진영을 회전시키는것은 커브딜레이가 꺼질때에 쓸거기때문에 오버라이딩
        base.Set_Curve_delay_Off();
        if(input_rotate)StartCoroutine("Rotate_Squad");
    }
    IEnumerator Rotate_Squad()//진영을 회전시킬때
    {
        if (!isCurving)
        {
            Set_Curving(true);
            GetComponent<BoxCollider>().enabled = true;
            rigid.isKinematic = false;

            //Debug.Log("Start Rotate_squad");
            Quaternion target_rot = Quaternion.identity;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

            if (curve_isright)
                target_rot = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);
            else
                target_rot = Quaternion.Euler(0, transform.eulerAngles.y - 90, 0);

            Set_speed(speed - rotateDecelSpeed);
            float max_time = rotateDecelTime;
            for (float f = 0; f < max_time; f += Time.deltaTime)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, target_rot, f / max_time);
                Set_Direction();
                if (f / rotateDecelTime > 0.5f && isCurving)
                {
                    Set_Curving(false);
                }
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = target_rot;

            rigid.isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
        }
        yield return null;
    }

    public override void Bounce_byObject(Vector3 contactPoint)
    {
        base.Bounce_byObject(contactPoint);
    }


}