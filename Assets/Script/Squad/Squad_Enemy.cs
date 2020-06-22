using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Enemy : Squad {

    Squad player;

    [Space(15)]
    [Header("Enemy Option")]
    public bool isAttack;
    public float rot_speed;
    float min_dis=1.5f;

    void Start()
    {
        isEnemy = true;
        player = GameObject.FindObjectOfType<Squad_Player>();
        Set_Attack(true);
        rotation_speed = rot_speed;
        Set_Active(true);
    }
    public override void Curve(Vector3 vec)
    {
        if (Vector3.Distance(transform.position, vec) < min_dis) return;
        rot_target.position = transform.position;
        rot_target.LookAt(vec);
        rot_target.eulerAngles = new Vector3(0, rot_target.eulerAngles.y, 0);
        Set_Curving(true);
    }

    public void Set_Attack(bool value)
    {
        if(value)
        {
            isAttack = true;
            StartCoroutine("Attack_to_player");

        }
        else
        {
            isAttack = false;
        }
    }

    IEnumerator Attack_to_player()
    {
        while(isAttack)
        {
            Curve(player.transform.position);
            
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    IEnumerator Sturn_timer()
    {
        yield return new WaitForSeconds(sturn_duration);
        Sturn(false);
    }
    
}
