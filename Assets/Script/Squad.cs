using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [Header("분대의 최대속도 관련")]
    public float max_straight_speed_persecond;
    public float max_curve_speed_persecond;
    public float max_dash_speed_persecond;
    public float max_knockback_speed_persecond;
     float max_straight_speed;
     float max_curve_speed;
     float max_dash_speed;
     float max_knockback_speed;


    [Header("분대의 가속도 관련")]
    public float accel_persecond;
    public float curve_decel_persecond;
    float accel;
    float curve_decel;

    [Header("각도 관련")]
    public Transform rot_target;
    public float rot_clamp;
    public float rot_speed;

    [Space(20)]
    protected float speed;
    protected bool isDash;
    bool isActive;
    bool isCurving;
    bool isKnockback;
    bool isTurnback;
    int max_dashcount=8;
    int dashcount=0;
    int max_knockbackcount = 3;
    int knockbackcount = 0;

    void Awake()
    {
        float f = Application.targetFrameRate;
        speed = 0;
        Set_Active(true);
        max_straight_speed= max_straight_speed_persecond / f;
        max_curve_speed = max_curve_speed_persecond / f;
         max_dash_speed = max_dash_speed_persecond / f;
         max_knockback_speed = max_knockback_speed_persecond / f;
        accel = accel_persecond / f;
        curve_decel = curve_decel_persecond / f;
    }

    public void Sum_speed(float value)
    {
        if(isKnockback)
        {
            isCurving = false;
            if (knockbackcount == 0) Knockback(false);
            speed = max_knockback_speed;
        }
        else if(isDash)
        {
            isCurving = false;
            if (dashcount == 0) Dash(false);
            speed = max_dash_speed;
        }
        else if(isCurving)
        {
            speed = Mathf.Clamp(speed, 0, max_straight_speed);
            if (speed > max_curve_speed) speed -= curve_decel;
            else speed += accel;
        }
        else
        {
            speed += value;
            speed = Mathf.Clamp(speed, 0, max_straight_speed);
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
        while(isActive)
        {
            Sum_speed(accel);
            transform.Translate(0, 0, speed);

            if(isTurnback)
            {
                for(int i=0; i<1; i++)
                {
                    transform.Rotate(0, 180, 0);
                }

                Turnback(false);
            }
            else if(isCurving)
            {
                Quaternion q = Quaternion.Slerp(transform.rotation, rot_target.rotation, rot_speed);
                Quaternion ori_q = transform.rotation;
                transform.rotation = q;

                if (Mathf.Abs(Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(ori_q.eulerAngles.y)) < rot_clamp)
                    isCurving = false;
            }

            dashcount--;
            knockbackcount--;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}
