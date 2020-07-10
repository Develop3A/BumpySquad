using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_Generator : CsvReader {

    public bool act = false;
    void Update()
    {
        if (act)
        {
            act = false;
            Read();
        }
    }
    void Start()
    {
        //Read();
    }

    public override void Read()
    {
        base.Read();

        for (int column = 1; column < all_words.GetLength(1); column++)
        {
            GameObject g;
            List<string> values = new List<string>();

            if (all_words[0, column] == "" | all_words[0, column] == null)
            {
                //Debug.Log("end");
                break;
            }
            for (int row = 0; row < all_words.GetLength(0); row++)
            {
                values.Add(all_words[row, column]);
                //Debug.Log(all_words[row, column]);
            }

            //Pop<string>(values)
            g = Instantiate(objNumberManager.instance.Find_Prefab(Pop<string>(values)), null);//A
            string parent_name = Pop<string>(values);//B
            float x, y, z;

            x = float.Parse(Pop<string>(values));//C
            y = float.Parse(Pop<string>(values));//D
            z = float.Parse(Pop<string>(values));//E
            g.transform.position = new Vector3(x, y, z);

            x = float.Parse(Pop<string>(values));//F
            y = float.Parse(Pop<string>(values));//G
            z = float.Parse(Pop<string>(values));//H
            g.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));

            x = float.Parse(Pop<string>(values));//I
            y = float.Parse(Pop<string>(values));//J
            z = float.Parse(Pop<string>(values));//K
            g.transform.localScale = new Vector3(x, y, z);

            g.transform.parent = GameObject.FindWithTag(parent_name).transform;
        }

#if UNITY_EDITOR
        {
            //Debug.Log("this is Editor");
        }
#endif
#if UNITY_ANDROID
        {
            Destroy(this.gameObject);
        }
#endif
    }

}
