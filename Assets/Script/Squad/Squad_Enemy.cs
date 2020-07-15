using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squad_Enemy : Squad {

    Squad player;
    public Squad player_squad { get { return player; } }
    public bool _isActive { get { return isActive; } }
    NavMeshAgent nav;

    [Space(15)]
    [Header("Enemy Option")]
    [HideInInspector]public bool isMoving;
    public float Angular_speed= 45.0f;
    
    //이 적분대의 AI
    protected Enemy_AI AI;

    public override void Ready()
    {
        base.Ready();
        isEnemy = true;
        player = FindObjectOfType<Squad_Player>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = max_speed;
        nav.acceleration = accel;

        AI = GetComponent<Enemy_AI>();
        AI.Ready();
    }
    public override void Set_Active(bool value)
    {
        isActive = value;
        StartCoroutine("Active");
        Set_Move(value);
    }
    /*Curve 이전 각도 계산
public override void Curve(Vector3 vec)
{
    if (Vector3.Distance(transform.position, vec) < min_dis{최소거리}) return;

    rot_target.position = transform.position;
    rot_target.LookAt(vec);
    rot_target.eulerAngles = new Vector3(0, rot_target.eulerAngles.y, 0);
    Set_Curving(true);
}
    */

    public void Set_Move(bool value)
    {
        if(value)
        {
            isMoving = true;
            StartCoroutine("Move_to_player");
        }
        else
        {
            isMoving = false;
        }
    }
    public void Set_Nav_Destination(Vector3 pos)
    { 
        nav.SetDestination(pos);
    }
    IEnumerator Move_to_player()
    {
        /* 이전 플레이어 추적 코드
        while (isMoving)
        {
            if (!nav)
            {
                Debug.LogWarning("not have nav");
                StopCoroutine("Move_to_player");
            }
            if (!AI) 
            {
                AI.AI_Act();
            }
            else
            {
                Set_Nav_Destination(player.transform.position);
            }

            yield return new WaitForSeconds(1.0f);
        }
        */

        AI.AI_Act();

        yield return null;
    }

    protected override IEnumerator Active()
    {
        while(isActive)
        {
            if(isSturn)
                rigid.velocity = Vector3.zero;
            
            float x = 0; x = Mathf.Clamp(x, -max_speed, max_speed);
            float z = 0; z = Mathf.Clamp(z, -max_speed, max_speed);
            if (isMire)
            {
                x = Clamp_Mire_speed(x);
                z = Clamp_Mire_speed(z);
            }
            rigid.velocity = new Vector3(x, rigid.velocity.y, z);

            yield return new WaitForEndOfFrame();
        }
    }

    public override void Set_Knockback(bool value, float time, float knockback_speed, float sturn_time, Transform player)
    {
        base.Set_Knockback(value, time, knockback_speed, sturn_time, player);
        if(value)
        {
            nav_Sturn(true);
        }
    }
    public void nav_Sturn(bool value)
    {
        //스턴일때 네비게이션을 멈춤
        nav.isStopped = value;
        if (value)
        {
            Set_Active(false);
            nav.velocity = Vector3.zero;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            //nav.angularSpeed = 0.0f;
        }
        else
        {
            //nav.angularSpeed = Angular_speed;
        }
    }
    public override IEnumerator Sturn()
    {
        float present_time = Time.time;
        //Debug.Log(sturn_duration);
        while (Time.time < present_time + sturn_duration)
        {
            nav_Sturn(true);
            yield return new WaitForEndOfFrame();
        }
        Set_Sturn(false);
        nav_Sturn(false);
        Set_Active(true);

        yield return null;
    }

}
