using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFollow : MonoBehaviour {
    
    Transform target;
    Image img;
    float max_hp;
    public Soldier s;

    void Start()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        target = s.transform;
        max_hp = s.Get_hp();
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
        try
        {
            if (target == null)
            {
                Destroy(this.gameObject);
            }
            Vector3 v = target.position;
            transform.position = new Vector3(v.x, v.y + 2, v.z);
            transform.eulerAngles = new Vector3(80, target.eulerAngles.y, target.eulerAngles.z);
            img.fillAmount = s.Get_hp() / max_hp;
        }
        catch
        {

            Destroy(this.gameObject);
        }
    }
}
