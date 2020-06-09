using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    public Squad Squad;
    public float max_hp = 10.0f;
    public float attack_range;
    public float attack_speed = 1.0f;

    protected float hp;
    protected bool isFighting;

    public bool isEnemy;

    void Awake()
    {
        hp = max_hp;
        Set_Fighting(true);
        GameManager.gm.Gen_hp_text(GetComponent<Soldier>());
    }

    public void Set_Fighting(bool value)
    {
        if (value)
        {
            isFighting = true;
            StartCoroutine("Fighting");
        }
        else
        {
            isFighting = false;
        }
    }
    IEnumerator Fighting()
    {
        while (isFighting)
        {
            Attack();
            yield return new WaitForSeconds(attack_speed);
        }
        yield return null;
    }
    public virtual void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attack_range);

        foreach(Collider c in colliders)
        {
            try
            {
                Soldier s = c.gameObject.GetComponent<Soldier>();


                if (!isEnemy & s.isEnemy)
                {
                    //Debug.Log(s.gameObject.name);
                    s.Sum_hp(-1);
                    break;
                }
                else if (isEnemy & !s.isEnemy)
                {
                    //Debug.Log(s.gameObject.name);
                    s.Sum_hp(-1);
                    break; 
                }
            }
            catch
            {
                continue;
            }
        }

    }

    public void Sum_hp(float value)
    {
        hp += value;

        if (hp <= 0) Death();
    }
    public float Get_hp()
    {
        return hp;
    }

    protected void Death()
    {
        Destroy(this.gameObject);
    }

}
