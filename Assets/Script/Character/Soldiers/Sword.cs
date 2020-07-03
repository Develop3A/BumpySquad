using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Soldier {

    public override void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attack_range);

        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (!isEnemy & s.isEnemy)
                {
                    //Debug.Log(s.gameObject.name);
                    s.Sum_hp(-attack_damage);
                    anim.SetTrigger("Attack");
                    Set_Fighting(false);
                    Invoke("attack_delay", attack_speed);
                    break;
                }
                else if (isEnemy & !s.isEnemy)
                {
                    //Debug.Log(s.gameObject.name);
                    s.Sum_hp(-attack_damage);
                    anim.SetTrigger("Attack");
                    Set_Fighting(false);
                    Invoke("attack_delay", attack_speed);
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
