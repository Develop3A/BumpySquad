using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShakeOff : Skill {

    public float turnback_knockback_range = 3;
    float turnback_knockback_time;
    float turnback_knockback_speed;

    protected override void First()
    {
        base.First();

        Skill_Number = 1;
        cooltime = 15;

        turnback_knockback_time = 0.1f;
        turnback_knockback_speed = -30.0f;
    }

    public override void Use()
    {
        base.Use();
        StartCoroutine("Use_ShakeOff");
    }

    IEnumerator Use_ShakeOff()
    {
        List<Squad> enemies = new List<Squad>();
        squad.Contact_check(out enemies);
        foreach(Squad enemy in enemies)
        {
            enemy.Set_Knockback(true, turnback_knockback_time, turnback_knockback_speed, 0, transform);
        }
        yield return null;
        //foreach ()
    }
}
