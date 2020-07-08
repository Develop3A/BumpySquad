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
    public bool isMoving;

    [Space(20)]
    [Header("인공지능 타입 - 공백 시 기본 AI")]
    public Enemy_AI AI;

    public override void Ready()
    {
        base.Ready();
        isEnemy = true;
        player = FindObjectOfType<Squad_Player>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = max_straight_speed_persecond;
        nav.acceleration = accel;

        AI = GetComponent<Enemy_AI>();
        AI.Ready();
    }
    public override void Set_Active(bool value)
    {
        isActive = value;
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
            
            float x = 0; x = Mathf.Clamp(x, -max_straight_speed_persecond, max_straight_speed_persecond);
            float z = 0; z = Mathf.Clamp(z, -max_straight_speed_persecond, max_straight_speed_persecond);
            if (isMire)
            {
                x = Clamp_Mire_speed(x);
                z = Clamp_Mire_speed(z);
            }
            rigid.velocity = new Vector3(x, rigid.velocity.y, z);

            yield return new WaitForEndOfFrame();
        }
    }

    public void nav_Sturn(bool value)
    {
        //스턴일때 네비게이션을 멈춤
        if(value)
        {
            nav.velocity = Vector3.zero;
            nav.SetDestination(transform.position);
            Set_Move(false);
        }
        else
        {
            //Set_Move를 안썼을경우에 대비해서 써둠.
            Set_Move(true);
        }
    }
    public override IEnumerator Sturn()
    {
        nav_Sturn(true);
        yield return new WaitForSeconds(sturn_duration);
        Set_Sturn(false);
        nav_Sturn(false);
        yield return null;
    }

}
