using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMan : Soldier
{

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
