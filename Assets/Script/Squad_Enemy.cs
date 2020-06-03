﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Enemy : Squad {

    Squad player;

    public bool isAttack;

    void Start()
    {
        player = GameObject.FindObjectOfType<Squad_Player>();
        Set_Attack(true);
    }
    public override void Curve(Vector3 vec)
    {
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

            yield return new WaitForEndOfFrame();
        }
    }
}
