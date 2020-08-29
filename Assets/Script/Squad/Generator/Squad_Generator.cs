using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Generator : MonoBehaviour {

    public GameObject[] soldier_type;
    public Transform Soldiers_parent;

    public int[] type_id = new int[9];

    void Awake()
    {
        Ready();
    }

    protected virtual void Ready()
    {
        if (Soldiers_parent == null) Soldiers_parent = transform;
        Gen_Squad();
    }

    public void Gen_Squad()
    {
        for(int i =0; i<9; i++)
        {
            float x=0, z=0;

            if (i == 0 | i == 3 | i == 6)
            {
                x = -1.25f;
            }
            else if (i == 1 | i == 4 | i == 7)
            {
                x = 0.0f;
            }
            else
            {
                x = 1.25f;
            }
            if(i ==0| i ==1| i ==2)
            {
                z = 1.25f;
            }
            else if(i ==3| i ==4| i ==5)
            {
                z = 0.0f;
            }
            else
            {
                z = -1.25f;
            }

            GameObject g = null;

            if(type_id[i] >0)g = Instantiate(soldier_type[type_id[i]-1], Soldiers_parent);

            if (g == null) continue;

            g.transform.position = new Vector3(Soldiers_parent.position.x + x, Soldiers_parent.position.y, Soldiers_parent.position.z + z);
            Soldiers_parent.GetComponent<Squad>().Set_soldier_num(g, i);
        }

    }

}
