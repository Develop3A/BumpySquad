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

    [Header("지속시간 관련")]
    public float dash_time;
    public float knockback_time;
    public float sturn_time;

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
    Rigidbody rigid;
    Soldier[] soldiers = new Soldier[9];

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        float f = Application.targetFrameRate;
        speed = 0;
        accel = accel_ / f;
        curve_decel = curve_decel_ / f;
    }

    public void Set_soldier_num(GameObject soldier_obj,int num)
    {
        soldiers[num] = soldier_obj.GetComponent<Soldier>();
    }

    public void Sum_speed(float value)
    {
        if(isKnockback)
        {
            isCurving = false;
            speed = max_knockback_speed_persecond;
        }
        else if(isDash)
        {
            isCurving = false;
            speed = max_dash_speed_persecond;
        }
        else if(isCurving )
        {
            if (speed > max_curve_speed_persecond) speed -= curve_decel;
            else speed += accel;
            speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond);
        }
        else if(isContact)
        {
            if (speed > max_contact_speed_persecond) speed -= curve_decel;
            else speed += accel;
            speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond);
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
    #region 플레이어의 특수상태나 스킬
    public void Dash(bool value)
    {
        if (!value)
        {
            if (!IsInvoking("Dash_Off")) Invoke("Dash_Off", dash_time);
        }
        if (isDash) return;
        else if (value)
        {
            if (!IsInvoking("Dash_Off")) Invoke("Dash_Off", dash_time);
            isDash = true;
        }
    }
    public void Dash_Off()
    {
        isDash = false;
    }

    public void Knockback(bool value)
    {
        if(!value)
        {
            if (!IsInvoking("Knockback_Off")) Invoke("Knockback_Off", knockback_time);
        }
        if (isKnockback) return;
        else if(value)
        {
            if (!IsInvoking("Knockback_Off")) Invoke("Knockback_Off", knockback_time);
            isKnockback = true;
            Sturn(true);
        }
    }
    public void Knockback_Off()
    {
        isKnockback = false;
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
    public void Strun_soldiers(float st)
    {
        for(int i = 0; i<soldiers.Length; i++)
        {
            if (soldiers[i] == null) continue;
            else
            {
                soldiers[i].soldier_sturn_time = sturn_time;
                soldiers[i].StartCoroutine("Soldier_Sturn");
            }
        }
    }

    public void Turnback(bool value)
    {
        if (!value)
        {
            isTurnback = false;
        }

        if (isTurnback) return;
        else if (value)
        {
            isCurving = false;
            isTurnback = true;
        }
    }
#endregion

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
            if(isSturn)
            {
                yield return new WaitForSeconds(sturn_time);
                speed = 0.0f;
                Sturn(false);
            }

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
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}
