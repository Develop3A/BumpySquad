using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squad_Enemy : Squad {

    Squad player;
    NavMeshAgent nav;

    [Space(15)]
    [Header("Enemy Option")]
    public bool isAttack;
    public float rot_speed;

    public override void Ready()
    {
        base.Ready();
        isEnemy = true;
        player = FindObjectOfType<Squad_Player>();
        nav = GetComponent<NavMeshAgent>();
        rotation_speed = rot_speed;
    }
    public override void Set_Active(bool value)
    {
        isActive = value;
        Set_Move(value);
    }
    public override void Curve(Vector3 vec)
    {
        /*
         * 이전 각도 계산
         * 
        if (Vector3.Distance(transform.position, vec) < min_dis{최소거리}) return;
        
        rot_target.position = transform.position;
        rot_target.LookAt(vec);
        rot_target.eulerAngles = new Vector3(0, rot_target.eulerAngles.y, 0);
        Set_Curving(true);
        */
    }
    
    public void Set_Move(bool value)
    {
        if(value)
        {
            isAttack = true;
            StartCoroutine("Move_to_player");
        }
        else
        {
            isAttack = false;
        }
    }

    IEnumerator Move_to_player()
    {
        while (isAttack)
        {
            if (!nav)
            {
                Debug.Log("not have nav");
                StopCoroutine("Move_to_player");
            }
            nav.SetDestination(player.transform.position);

            yield return new WaitForSeconds(1.0f);
        }
    }
    
    IEnumerator Sturn_timer()
    {
        yield return new WaitForSeconds(sturn_duration);
        Debug.Log("Sturn_timer");
        Set_Sturn(false);
    }
    
}
