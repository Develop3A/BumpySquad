using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : Soldier {

    [Header("캐스터 옵션")]
    public GameObject fireball;

    public override void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attack_range);
        GameObject target = null;

        foreach (Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (Detect_Enemy(s))
                {
                    if (target == null)
                    {
                        target = s.gameObject;
                    }
                    else if (Vector3.Distance(transform.position, target.transform.position) <
                    Vector3.Distance(transform.position, s.transform.position))
                    {
                        target = s.gameObject;
                    }
                    Set_Fighting(false);
                    Invoke("attack_delay", attack_speed);
                    break;
                }
                /*
                else if (isEnemy & !s.isEnemy)
                {
                    if (target == null)
                    {
                        target = s.gameObject;
                    }
                    else if (Vector3.Distance(transform.position, target.transform.position) <
                    Vector3.Distance(transform.position, s.transform.position))
                    {
                        target = s.gameObject;
                    }
                    Set_Fighting(false);
                    Invoke("attack_delay", attack_speed);
                    break;
                }
                */

            }
            catch
            {
                continue;
            }
        }

        if (target != null)
        {
            GameObject g = Instantiate(fireball, null);
            g.transform.position = transform.position;
            Casters_Fireball a = g.GetComponent<Casters_Fireball>();
            a.damage = attack_damage;
            a.t = target;
            a.StartCoroutine("to_t");
            anim.SetTrigger("Attack");
        }
    }
}
