using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : Soldier {

    [Header("방패병 옵션")]
    public float attack_sturn_duration = 2.0f;

    public override void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attack_range);

        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (Detect_Enemy(s))
                {
                    //Debug.Log(s.gameObject.name);
                    s.Sum_hp(-attack_damage);
                    anim.SetTrigger("Attack");
                    Set_Fighting(false);
                    Invoke("attack_delay", attack_speed);
                    s.soldier_sturn_time = attack_sturn_duration;
                    s.StartCoroutine("Soldier_Sturn");
                    break;
                }
            }
            catch
            {
                continue;
            }
        }
    }
}
