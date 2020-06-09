using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archers_arrow : MonoBehaviour {
    
    public GameObject t;
    public float damage;

    IEnumerator to_t()
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
            transform.LookAt(t.transform);
            transform.position = Vector3.Lerp(transform.position, t.transform.position, i * 0.1f);
            }
            catch
            {
                Destroy(gameObject, 0.1f);
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        try
        {
            t.GetComponent<Soldier>().Sum_hp(-damage);
        Destroy(gameObject, 0.1f);
        }
        catch
        {
            Destroy(gameObject, 0.1f);
        }
        yield return null;
    }
}
