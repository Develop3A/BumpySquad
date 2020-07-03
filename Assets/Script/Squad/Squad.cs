using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : Squad_property
{

    //[Header("Squad Option")]
    //[Header("분대의 최대속도 관련")]
    //public float max_straight_speed_persecond;
    //public float max_curve_speed_persecond;
    //public float max_dash_speed_persecond;
    //public float max_knockback_speed_persecond;
    //public float max_contact_speed_persecond;
    //[Range(0,1)]public float max_mire_speed_ratio;


    //[Header("분대의 가속도 관련")]
    //public float accel_;
    //public float curve_decel_;
    float accel;
    float curve_decel;

    //[Header("분대의 접촉판정 관련")]
    //public float yellowboxsize;
    //public float plus_z;
    Vector3 box_pos;
    Vector3 box_size;

    //[Header("각도 관련")]
    //public Transform rot_target;
    //public float curve_delay_time;
    //public bool isCurve_delay;

    //[Header("지속시간 관련")]
    //public float dash_time;
    //public float knockback_time;
    //public float sturn_time;

    //    [Header("넉백 관련")]

    #region 보이지 않는 값
    [Space(10)]
   [SerializeField] protected float speed;
    protected float knockback_time;
    protected float max_knockback_speed_persecond;
    protected float rotation_speed = 0.025f;
    protected bool curve_isright;
    protected bool isEnemy;
    protected bool isDash;
    protected bool isContact;
    protected bool isColliderContact;
    protected bool isSturn;
    bool isMire;
    bool isActive;
    bool isCurving;
    bool isKnockback;
    bool isTurnback;
    Rigidbody rigid;
    protected Soldier[] soldiers = new Soldier[9];
#endregion

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        float f = Application.targetFrameRate;
        speed = 0;
        accel = accel_ ;
        curve_decel = curve_decel_ ;

        box_size = new Vector3(yellowboxsize, yellowboxsize, yellowboxsize + plus_z);
    }

    public void Set_soldier_num(GameObject soldier_obj,int num)
    {
        soldiers[num] = soldier_obj.GetComponent<Soldier>();
        soldier_obj.GetComponent<Soldier>().Squad = this;
    }

    public void Sum_speed(float value)
    {
        if(isKnockback)
        {
            isCurving = false;
            if (!IsInvoking("Knockback_Off"))
            {
                Invoke("Knockback_Off", knockback_time);
            }
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
            if(isMire) speed = Mathf.Clamp(speed, 0, Clamp_Mire_speed(max_straight_speed_persecond));
            else speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond); ;
        }
        else if(isContact& isColliderContact)
        {
            if (speed > max_contact_speed_persecond) speed = max_contact_speed_persecond;
            else speed += accel;
            speed = Mathf.Clamp(speed, 0, max_contact_speed_persecond);
        }
        else
        {
            speed += value;
            if (isMire) speed = Mathf.Clamp(speed, 0, Clamp_Mire_speed(max_straight_speed_persecond));
            else speed = Mathf.Clamp(speed, 0, max_straight_speed_persecond); ;
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
        if(!isEnemy)
        {
            if (value) rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            else rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    public virtual void Curve(Vector3 vec)
    {
        
    }
    public void Contact_Check()
    {
        box_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + plus_z);
        Collider[] colliders = Physics.OverlapBox(box_pos, box_size * 0.5f, transform.rotation);

        bool contact = false;

        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (!isEnemy & s.isEnemy)
                {
                    contact = true;
                    break;
                }
                else if (isEnemy & !s.isEnemy)
                {
                    contact = true;
                    break;
                }
            }
            catch
            {
                continue;
            }

            isContact = contact;
            if (!isContact) isColliderContact = false;
        }
    }
    #region 플레이어의 특수상태나 스킬
    public void Dash(bool value,float duration)
    {
        if (!value)
        {
            if (!IsInvoking("Dash_Off")) Invoke("Dash_Off", duration);
        }
        if (isDash) return;
        else if (value)
        {
            if (!IsInvoking("Dash_Off")) Invoke("Dash_Off", duration);
            isDash = true;
        }
    }
    public void Dash_Off()
    {
        isDash = false;
    }

    public void Knockback(bool value, float time, float knockback_speed,float sturn_time)
    {
        if(!value)
        {
            if (!IsInvoking("Knockback_Off")) Invoke("Knockback_Off", time);
        }
        if (isKnockback) return;
        else if(value)
        {
            //Debug.Log(gameObject.name + time.ToString() + knockback_speed.ToString() + sturn_time.ToString());
            isKnockback = true;
            max_knockback_speed_persecond = knockback_speed;
            knockback_time = time;
            sturn_duration = sturn_time;
            Invoke("Sturn", time);
        }
    }
    public void Knockback(bool value)
    {
        if(value)
        {
            Debug.LogError("예전 넉백 함수를 쓰고있습니다");
            return;
        }
        if (!value)
        {
            if (!IsInvoking("Knockback_Off")) Invoke("Knockback_Off", knockback_time);
        }
        if (isKnockback) return;
        else if (value)
        {
            isKnockback = true;
            Invoke("Sturn", knockback_time);
        }
    }
    public void Knockback_Off()
    {
        isKnockback = false;
    }

    public void Sturn()
    {
        Sturn(true);
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
            Sturn_soldiers();
        }
    }
    public void Sturn_soldiers()
    {
        for(int i = 0; i<soldiers.Length; i++)
        {
            if (soldiers[i] == null) continue;
            else
            {
                soldiers[i].Set_Fighting(false);
                soldiers[i].soldier_sturn_time = sturn_duration;
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

    public void Set_Mire(bool value)
    {
        isMire = value;
    }
    float Clamp_Mire_speed(float f)
    {
        return f * max_mire_speed_ratio;
    }
#endregion

    protected void Set_Curve_delay_On()
    {
        isCurve_delay = true;
        CancelInvoke("Set_Curve_delay_Off");
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
            rigid.velocity = transform.forward * speed;

            if (isSturn)
            {
                yield return new WaitForSeconds(sturn_duration);
                speed = 0.0f;
                Sturn(false);
                Knockback(false);
            }

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
                    if (curve_isright &&!isCurve_delay) transform.Rotate(Vector3.up * rotation_speed);
                    else if(!isCurve_delay) transform.Rotate(Vector3.up *-rotation_speed);
                }
            }
            Contact_Check();

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }



    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero+ new Vector3(0,0,plus_z),box_size);
    }
}
