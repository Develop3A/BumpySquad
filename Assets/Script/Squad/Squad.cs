using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : Squad_property
{
    Vector3 box_pos;
    Vector3 box_size;
    Vector3 knockback_dir;

    #region 보이지 않는 값
    [Space(10)]
    [HideInInspector]
    public float speed;
    [HideInInspector] public Skill[] skills;
    [HideInInspector] public Rigidbody rigid;
    //public float back_;

    protected float knockback_time; //넉백되는 시간
    protected float max_knockback_speed_persecond; //넉백되는 속도 (초당 N만큼)
    protected float rotation_speed = 0.025f;
    protected bool curve_isright;
    protected bool isEnemy;
    protected bool isContact;
    protected bool isColliderContact;
    protected bool isSturn;
    protected bool isMire;
    protected bool isActive;
    protected bool isCurving;
    protected bool isKnockback;
    protected enum Direction { front, back, left, right }
    protected Direction direction = Direction.front;
    protected Soldier[] soldiers = new Soldier[9];
    #endregion

    public virtual void Ready()
    {
        rigid = GetComponent<Rigidbody>();
        if (!rot_target) rot_target = transform.GetChild(0);
        speed = 0;

        box_size = new Vector3(yellowboxsize, yellowboxsize, yellowboxsize + plus_z);

        foreach (Soldier s in soldiers)
            s.Ready();
    }

    public virtual void Set_Active(bool value)
    {
        Debug.Log("스쿼드 : 활성화 됐을 때의 코드를 작성해 주세요.");
    }

    public void Set_soldier_num(GameObject soldier_obj, int num)
    {
        soldiers[num] = soldier_obj.GetComponent<Soldier>();
        soldier_obj.GetComponent<Soldier>().Squad = this;
    }
    public Soldier[] Get_soldier()
    {
        return soldiers;
    }
    public bool Front_check()
    {
        bool contact = false;

        Soldier[] soldiers = Get_soldier();
        foreach(Soldier s in soldiers)
        {
            if(s.Front_contact())
            {
                contact = true;
                break;
            }
        }
        return contact;
    }

    public void Clamp_speed(float value)
    {

    }
    public float Update_speed(float value)
    {
        float speed_ = speed;
        if (isCurving && !isEnemy)
        {
            speed_ += value;
            if (speed_ > maxSpeed - rotateDecelSpeed)
            {
                Sum_speed(turnDecelSpeed);
            }

            speed_ = Mathf.Clamp(speed_, 0, maxSpeed); ;
        }
        else
        {
            speed_ += value;
            speed_ = Mathf.Clamp(speed_, 0, maxSpeed); ;
        }
        if (isMire) return speed_ * mire_speed_ratio;
        else return speed_;
    }
    public void Sum_speed(float value)
    {
        speed += value;
    }
    public void Sum_speed(float value,float min)
    {
        speed += value;
        speed = Mathf.Clamp(speed, min, maxSpeed);
    }
    public virtual void Set_speed(float value)
    {
        speed = value;
        speed = Mathf.Clamp(speed, speed, value);
    }
    public float Cal_accel()
    {
        return (maxSpeed / maxSpeedReachTime) / Application.targetFrameRate;
    }

    protected void Set_Direction(Direction d)
    {
        Sum_speed(-turnDecelSpeed,minSpeed);

        direction = d;

        Vector3 dir =Vector3.zero;
        if (d == Direction.front) dir = new Vector3(0, 0, 0);
        else if (d == Direction.back) dir = new Vector3(0, 180, 0);
        else if (d == Direction.left) dir = new Vector3(0, 270, 0);
        else if (d == Direction.right) dir = new Vector3(0, 90, 0);
            foreach (Soldier s in Get_soldier())
            {
            s.gameObject.transform.eulerAngles = dir;
        }
    }
    Vector3 Get_Direction()
    {
        switch (direction)
        {
            case Direction.front:
                return Vector3.forward;
            case Direction.back:
                return Vector3.back;
            case Direction.left:
                return Vector3.left;
            case Direction.right:
                return Vector3.right;
            default:
                Debug.Log("None Direction");
                break;
        }
        Debug.Log("None Direction");
        return Vector3.zero;
    }
    public void Set_Curving(bool value)
    {
        isCurving = value;
        /*
        if(!isEnemy)
        {
            if (value) rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            else rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        */
    }
    public virtual void Curve(Vector3 vec)
    {

    }
    public void Set_Collider_Contact(bool value)
    {
        isColliderContact = value;
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
    public void Contact_check(out List<Squad> ss)
    {
        //뒤로 돌아스킬을 사용할때 접촉한 적이 있는지 판단하고 넉백을 주기위한 함수임.
        box_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + plus_z);
        Collider[] colliders = Physics.OverlapBox(box_pos, box_size * 0.5f, transform.rotation);

        bool contact = false;
        ss = new List<Squad>();
        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (!isEnemy & s.isEnemy)
                {
                    contact = true;
                    //s가 적 용병일경우 
                    if (ss.Count != 0)
                    {
                        bool dupli = false;
                        foreach (Squad squad in ss)
                        {
                            if (s.Squad != squad) continue;
                            else dupli = true;
                        }

                        if (!dupli)
                        {
                            ss.Add(s.Squad);
                        }
                    }
                    else
                    {
                        ss.Add(s.Squad);
                    }
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

    public virtual void Set_Knockback(bool value, float time, float knockback_speed, float sturn_time, Transform player)
    {
        if (!value)
        {
            if (!IsInvoking("Knockback_Off")) Invoke("Knockback_Off", time);
        }
        if (isKnockback) return;
        else if (value)
        {
            GameObject g = new GameObject();
            g.transform.position = transform.position;
            g.transform.LookAt(player);
            knockback_dir = -g.transform.forward;
            //knockback_dir = player.forward;
            max_knockback_speed_persecond = knockback_speed;
            knockback_time = time;
            sturn_duration = sturn_time;
            isKnockback = true;
            StartCoroutine("Knockback");
        }
    }
    public virtual void Set_Knockback_Off()
    {
        isKnockback = false;
    }

    public void Set_Sturn()
    {
        Set_Sturn(true);
    }
    public void Set_Sturn(bool value)
    {
        if (!value)
        {
            isSturn = false;
        }

        if (isSturn) return;
        else if (value)
        {
            isSturn = true;
            StartCoroutine("Sturn");
            Sturn_soldiers();
        }
    }
    public void Sturn_soldiers()
    {
        for (int i = 0; i < soldiers.Length; i++)
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


    public void Set_Mire(bool value)
    {
        isMire = value;
    }
    protected float Clamp_Mire_speed(float f)
    {
        return f * mire_speed_ratio;
    }
    #endregion

    protected void Set_Curve_delay_On()
    {
        is_rotate_able = true;
        CancelInvoke("Set_Curve_delay_Off");
    }
    protected virtual void Set_Curve_delay_Off()
    {
        is_rotate_able = false;
    }

    protected virtual IEnumerator Active()
    {
        while (isActive)
        {
            bool front = Front_check();
            if (!front)
            {
                accel = Cal_accel();
                speed = Update_speed(accel);
                if (speed == maxSpeed) collisionPower = true;
                else collisionPower = false;
                //float yv = rigid.velocity.y;
                SpeedBarManager.sbm.Refresh(speed / maxSpeed);
                Vector3 vel = Vector3.zero;
                vel = Get_Direction() * speed * Time.deltaTime;
                //rigid.velocity = new Vector3(vel.x, yv, vel.z);
                transform.position += vel;
            }
            Contact_Check();

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator Knockback()
    {
        float present_time = Time.time;
        while ((Time.time < present_time + knockback_time))
        {
            if (!isKnockback) break;
            rigid.velocity = knockback_dir * -max_knockback_speed_persecond * Application.targetFrameRate * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rigid.velocity = Vector3.zero;
        Set_Knockback_Off();
        Set_Sturn(true);
        yield return null;
    }
    public virtual IEnumerator Sturn()
    {
        if (isSturn)
        {
            yield return new WaitForSeconds(sturn_duration);
            speed = 0.0f;
            Set_Sturn(false);
        }
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, 0, plus_z), box_size);
    }
    */
    public virtual void Bounce_byObject(Vector3 contactPoint)
    {
        Debug.Log("qwe");
        Quaternion q = new Quaternion();
        q.SetLookRotation(contactPoint);
        Debug.Log(transform.eulerAngles);
        Debug.Log(q.eulerAngles);
        rigid.velocity = -q.eulerAngles 
            //* back_
            ;
    }
    
}