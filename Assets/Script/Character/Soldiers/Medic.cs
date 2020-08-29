using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Soldier {

    public override void Attack()
    {
        Soldier[] soldiers = Squad.Get_soldier();
        Soldier soldier = null;
        foreach (Soldier s in soldiers)
        {
            try
            {
                if(soldier == null)
                {
                    soldier = s;
                }
                else
                {
                    if(soldier.Get_hp()>s.Get_hp())
                    {
                        soldier = s;
                    }
                }
            }
            catch
            {
                continue;
            }
        }
        if (soldier != null)
        {
            //Debug.Log("Heal!");
            soldier.Sum_hp(attack_damage);
            anim.SetTrigger("Attack");
            Set_Fighting(false);
            Invoke("attack_delay", attack_speed);
        }
    }

}
