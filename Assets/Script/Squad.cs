using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [Header("Squad Option")]
    [Header("분대의 최대속도 관련")]
    public float max_straight_speed_persecond;
    public float max_curve_speed_persecond;
    public float max_dash_speed_persecond;
    public float max_knockback_speed_persecond;


    [Header("분대의 가속도 관련")]
    public float accel_;
    public float curve_decel_;
    float accel;
    float curve_decel;

    [Header("각도 관련")]
    public Transform rot_target;

    [Space(20)]
   [SerializeField] protected float speed;
    protected float rotation_speed = 0.025f;
    protected bool curve_isright;
    protected bool isEnemy;
    protected bool isDash;
    bool isActive;
    bool isCurving;
    bool isKnockback;
    bool isTurnback;
    int max_dashcount=8;
    int dashcount=0;
    int max_knockbackcount = 3;
    int knockbackcount = 0;
    Rigidbody rigid;
    
    /*
    [SerializeField] float z1, z2;
    [SerializeField] float time = 1.0f;
    bool timer_active=false;
    */

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        float f = Application.targetFrameRate;
        speed = 0;
        accel = accel_ / f;
        curve_decel = curve_decel_ / f;
    }

    public void Sum_speed(float value)
    {
        if(isKnockback)
        {
            isCurving = false;
            if (knockbackcount == 0) Knockback(false);
            speed = max_knockback_speed_persecond;
        }
        else if(isDash)
        {
            isCurving = false;
            if (dashcount == 0) Dash(false);
            speed = max_dash_speed_persecond;
        }
        else if(isCurving)
        {
            speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond);
            if (speed > max_curve_speed_persecond) speed -= curve_decel;
            else speed += accel;
        }
        else
        {
            speed += value;
            speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond);
        }
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
    }
    public virtual void Curve(Vector3 vec)
    {
        
    }
    public void Dash(bool value)
    {
        if (!value)
        {
            isDash = false;
        }

        if (isDash) return;
        else if (value)
        {
            dashcount = max_dashcount;
            isDash = true;
        }
    }
    public void Knockback(bool value)
    {
        if(!value)
        {
            isKnockback = false;
        }

        if (isKnockback) return;
        else if(value)
        {
            knockbackcount = max_knockbackcount;
            isKnockback = true;
        }
    }
    public void Turnback(bool value)
    {
        if(!value)
        {
            isTurnback = false;
        }

        if (isTurnback) return;
        else if(value)
        {
            isCurving = false;
            isTurnback = true;
        }
    }

    IEnumerator Active()
    {
        while (isActive)
        {
            Sum_speed(accel);
            rigid.velocity = transform.forward * speed;

            if (isTurnback)
            {
                for (int i = 0; i < 1; i++)
                {
                    transform.Rotate(0, 180, 0);
                }

                Turnback(false);
            }
            else if (isCurving)
            {
                if (isEnemy)
                {
                    Quaternion q = Quaternion.Slerp(transform.rotation, rot_target.rotation, rotation_speed);
                    Quaternion ori_q = transform.rotation;
                    transform.rotation = q;

                    if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(ori_q.eulerAngles.y)) < 0.1f)
                        isCurving = false;
                }
                else
                {
                    if (curve_isright) transform.Rotate(0, rotation_speed, 0);
                    else transform.Rotate(0, -rotation_speed, 0);
                }
            }

            dashcount--;
            knockbackcount--;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

    /*
    IEnumerator Timer()
    {
        if (!timer_active)
        {
            timer_active = true;
            z1 = transform.position.z;
            yield return new WaitForSeconds(time);
            z2 = transform.position.z;
        }
    }
    */
}
