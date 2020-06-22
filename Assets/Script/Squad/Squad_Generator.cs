using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad_Generator : MonoBehaviour {

    public GameObject sword,archer;
    public Transform Soldiers_parent;

    public string[] soldier_type = new string[9];

    void Awake()
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

            switch (soldier_type[i])
            {
                case "sword":
                    g = Instantiate(sword, Soldiers_parent);
                    break;
                case "Sword":
                    g = Instantiate(sword, Soldiers_parent);
                    break;
                case "archer":
                    g = Instantiate(archer, Soldiers_parent);
                    break;
                case "Archer":
                    g = Instantiate(archer, Soldiers_parent);
                    break;
            }

            if (g == null) continue;

            g.transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            GetComponent<Squad>().Set_soldier_num(g, i);
        }

    }

}
