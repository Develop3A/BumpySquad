using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{


    [Header("용병 기본 옵션")]
    public float max_hp = 10.0f;
    public float attack_damage;
    public float attack_range;
    public float attack_speed = 1.0f;
    public float soldier_sturn_time = 0.0f;


    [Space(20)]
    [Header("여긴 조작 안하셔도됩니다.")]
    public Squad Squad;
    protected bool isSturn;
    public Animator anim;

    protected float hp;
    protected bool isFighting;

    public bool isEnemy;

    void Awake()
    {
        hp = max_hp;
        Set_Fighting(true);
    }
    void Start()
    {
        PlayerManager.pm.Gen_hp_text(GetComponent<Soldier>());
    }

    public void Set_Fighting(bool value)
    {
        if (value == isFighting) return;
        else if (value)
        {
            isFighting = true;
            StartCoroutine("Fighting");
            StartCoroutine("Soldier_running");
        }
        else
        {
            StopCoroutine("Soldier_running");
            isFighting = false;
        }
    }
    IEnumerator Fighting()
    {
        while (isFighting)
        {
            Attack();
            yield return null;
        }
        yield return null;
    }
    public virtual void Attack()
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
                    Set_Fighting(false);
                    Invoke("attack_delay",attack_speed);
                    break;
                }
                else if (isEnemy & !s.isEnemy)
                {
                    //Debug.Log(s.gameObject.name);
                    s.Sum_hp(-attack_damage);
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
    protected void attack_delay()
    {
        if (!isSturn)
        {
            Set_Fighting(true);
        }
    }

    public IEnumerator Soldier_running()
    {
        bool co = true;
        bool updown = true;
        if (anim != null)
        {
            while (co)
            {
                if (updown)
                {
                    anim.transform.Translate(transform.position + Vector3.up * 0.2f);
                    updown = false;
                }
                else
                {
                    anim.transform.position = (transform.position);
                    updown = true;
                }

                yield return new WaitForSeconds(0.166f);
            }
        }
        yield return null;
    }
    public IEnumerator Soldier_Sturn()
    {
        if (!isSturn)
        {
            Set_Fighting(false);
            Color c = GetComponent<MeshRenderer>().materials[0].color;
            isSturn = true;
            GetComponent<MeshRenderer>().materials[0].color = Color.black;

            yield return new WaitForSeconds(soldier_sturn_time);

            isSturn = false;
            Set_Fighting(true);
            GetComponent<MeshRenderer>().materials[0].color = c;
        }
    }

    public void Sum_hp(float value)
    {
        hp += value;
        if (value < 0) hit();

        if (hp <= 0) Death();
    }
    void hit()
    {
        ParticleManager.pm.Gen_hit_particle(transform,0);
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
