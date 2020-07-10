using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hole : MonoBehaviour {

    void OnTriggerEnter(Collider c)
    {
        //Debug.Log(c.gameObject.name+ " in hole ");

        if(c.gameObject.tag == "Enemy"|| c.gameObject.tag == "Player")
        {
            try
            {
                Squad s = c.gameObject.GetComponent<Soldier>().Squad;
                s.Set_Mire(true);
            }
            catch
            {
                return;
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        //Debug.Log(c.gameObject.name + " out hole ");

        if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "Player")
        {
            try
            {
                Squad s = c.gameObject.GetComponent<Soldier>().Squad;
                s.Set_Mire(false);
            }
            catch
            {
                return;
            }
        }
    }


}
