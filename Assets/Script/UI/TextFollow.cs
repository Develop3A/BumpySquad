using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFollow : MonoBehaviour {
    
    Transform target;
    Text t;
    public Soldier s;

    void Start()
    {
        t = GetComponent<Text>();
        target = s.transform;
        //StartCoroutine("Active");
    }

    /*
    IEnumerator Active()
    {
        bool co = true;

        while(co)
        {
            if (target == null)
            {
                Destroy(this.gameObject);
                co = false;
                continue;
            }
            Vector3 v = target.position;
            transform.position = new Vector3(v.x, v.y + 1, v.z);
            transform.eulerAngles = new Vector3(80, target.eulerAngles.y, target.eulerAngles.z);
            t.text = s.Get_hp().ToString();
            yield return new WaitForEndOfFrame();
        }
    }
    */

    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        Vector3 v = target.position;
        transform.position = new Vector3(v.x, v.y + 1, v.z);
        transform.eulerAngles = new Vector3(80, target.eulerAngles.y, target.eulerAngles.z);
        t.text = s.Get_hp().ToString();
    }
}
