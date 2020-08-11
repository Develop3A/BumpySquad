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

    [Header("충돌박스")]
    public Vector3 box_size = new Vector3(1, 0.5f, 0.5f);
    public Vector3 box_center = new Vector3(0, 0, 0.3f);
    Vector3 center;


    [Space(20)]
    [Header("여긴 조작 안하셔도됩니다.")]
    public Squad Squad;
    protected bool isSturn;
    public Animator anim;
    public GameObject running_particle;

    protected float hp;
    protected bool isFighting;

    public bool isEnemy;

    public void Ready()
    {
        hp = max_hp;
        Set_Fighting(true);
        PlayerManager.pm.Gen_hp_text(GetComponent<Soldier>());
        if (running_particle)
        {
            GameObject g = Instantiate(running_particle, transform.position, Quaternion.identity, transform);
            g.transform.localEulerAngles = new Vector3(-90.0f, 90.0f, 0);
        }
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
                    Invoke("attack_delay", attack_speed);
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
                    anim.transform.position = transform.position + (Vector3.up * 0.2f) + (Vector3.up * -0.5f);
                    updown = false;
                }
                else
                {
                    anim.transform.position = transform.position + (Vector3.up * -0.5f);
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
        ParticleManager.pm.Gen_hit_particle(transform, 0);
    }
    public float Get_hp()
    {
        return hp;
    }

    protected void Death()
    {
        Destroy(this.gameObject);
    }


    public bool Front_contact()
    {
        bool contact = false;
        try
        {
            Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(box_center), box_size * 0.5f, transform.rotation);

            foreach (Collider c in colliders)
            {
                try
                {
                    Soldier s = c.gameObject.GetComponent<Soldier>();


                    if (!isEnemy & s.isEnemy)
                    {
                        contact = true;
                        //Debug.Log(c.gameObject.name);
                        //s가 적 용병일경우 
                        break;
                    }
                    else if (isEnemy & !s.isEnemy)
                    {
                        contact = true;
                        //Debug.Log(c.gameObject.name);
                        break;
                    }
                }
                catch
                {
                    if (!isEnemy && c.gameObject.tag == "rock")
                    {
                        contact = true;
                        //Debug.Log(c.gameObject.name);
                    }
                    continue;
                }
            }
        }
        catch
        {

        }

        return contact;
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero + new Vector3(box_center.x, box_center.y, box_center.z), box_size);
    }
    */
}
