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
    public float max_contact_speed_persecond;


    [Header("분대의 가속도 관련")]
    public float accel_;
    public float curve_decel_;
    float accel;
    float curve_decel;

    [Header("각도 관련")]
    public Transform rot_target;
    public float curve_delay_time;
    public bool isCurve_delay;

    [Space(20)]
   [SerializeField] protected float speed;
    protected float rotation_speed = 0.025f;
    protected bool curve_isright;
    protected bool isEnemy;
    protected bool isDash;
    protected bool isContact;
    protected bool isSturn;
    bool isActive;
    bool isCurving;
    bool isKnockback;
    bool isTurnback;
    int max_dashcount=8;
    int dashcount=0;
    int max_knockbackcount = 3;
    int knockbackcount = 0;
    Rigidbody rigid;

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
        else if(isCurving )
        {
            speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond);
            if (speed > max_curve_speed_persecond) speed -= curve_decel;
            else speed += accel;
        }
        else if(isContact)
        {
            speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond);
            if (speed > max_contact_speed_persecond) speed -= curve_decel;
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
            Sturn(true);
        }
    }
    public void Sturn(bool value)
    {
        if (!value)
        {
            isSturn = false;
        }

        if (isSturn) return;
        else if (value)
        {
            isSturn = true;
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

    protected void Set_Curve_delay_On()
    {
        isCurve_delay = true;
    }
    protected void Set_Curve_delay_Off()
    {
        isCurve_delay = false;
    }

    IEnumerator Active()
    {
        while (isActive)
        {
            Sum_speed(accel);
            rigid.velocity = transform.forward * speed ;

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
                    Quaternion q = Quaternion.Slerp(transform.rotation, rot_target.rotation, rotation_speed );
                    Quaternion ori_q = transform.rotation;
                    transform.rotation = q;

                    if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(ori_q.eulerAngles.y)) < 0.1f)
                        isCurving = false;
                }
                else
                {
                    if (curve_isright &&!isCurve_delay) transform.Rotate(0, rotation_speed , 0);
                    else if(!isCurve_delay) transform.Rotate(0, -rotation_speed, 0);
                }
            }

            dashcount--;
            knockbackcount--;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}
