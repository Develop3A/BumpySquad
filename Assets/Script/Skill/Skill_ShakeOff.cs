using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ShakeOff : Skill {

    public float shakeoff_knockback_range = 2.5f;
    float shakeoff_knockback_time;
    float shakeoff_knockback_speed;

    protected override void First()
    {
        base.First();

        Skill_Number = 1;
        cooltime = 15;

        shakeoff_knockback_time = 0.1f;
        shakeoff_knockback_speed = -50.0f;
        StartCoroutine("Cooltimer");
    }

    public override void Use()
    {
        base.Use();
        StartCoroutine("Use_ShakeOff");
    }

    IEnumerator Use_ShakeOff()
    {
        Collider[] contacts = Physics.OverlapSphere(transform.position, shakeoff_knockback_range);
        foreach (Collider c in contacts)
        {
            if (c.tag == "Enemy")
            {
                c.GetComponent<Soldier>().Squad.Set_Knockback(true, shakeoff_knockback_time, shakeoff_knockback_speed, 0, transform);
                //Debug.Log(c.gameObject.name);
            }
        }
        yield return null;
        //foreach ()
    }
}
