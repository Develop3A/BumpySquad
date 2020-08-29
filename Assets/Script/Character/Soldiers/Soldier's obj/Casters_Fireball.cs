using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casters_Fireball : MonoBehaviour {

    public GameObject t;
    public float damage;

    IEnumerator to_t()
    {
        Vector3 pos = t.transform.position;
        Vector3 origin_pos = transform.position;
        for (int i = 0; i < 30; i++)
        {
                transform.LookAt(pos);
                transform.position = Vector3.Lerp(origin_pos, pos, i * 1.0f/30.0f);
            yield return new WaitForEndOfFrame();
        }

        try
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

            foreach (Collider c in colliders)
            {
                try
                {
                    Soldier s = c.gameObject.GetComponent<Soldier>();


                    if(s.isEnemy == t.GetComponent<Soldier>().isEnemy)
                    {
                        s.Sum_hp(-damage);
                    }
                }
                catch
                {
                    continue;
                }
            }
            Destroy(gameObject);
        }
        catch
        {
            Destroy(gameObject);
        }
        yield return null;
    }
}
