using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_Generator : CsvReader {

    [Header("생성할 오브젝트 이름")]
    [SerializeField] public string Objectname;
    Vector3 gen_pos;

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

    }

    public void Editor_generate()
    {
        GameObject g =  Instantiate(objNumberManager.instance.Find_Prefab(Objectname));
        g.transform.parent = GameObject.FindWithTag("objs").transform;
        g.transform.position = gen_pos;
    }
    public GameObject Editor_generate(GameObject obj)
    {
        GameObject g = Instantiate(obj);
        g.transform.parent = GameObject.FindWithTag("objs").transform;
        g.transform.position = new Vector3();
        g.transform.position = gen_pos;
        return g;
    }
    public void ReadAndGenerateAll()
    {
        GameObject objs = GameObject.FindWithTag("objs");

        while(objs.transform.childCount > 0)
        {
            Destroy(objs.transform.GetChild(0));
        }
        Read();
    }
    
#if UNITY_EDITOR
void OnDrawGizmos()
    {
        //Camera.current.transform.position, Camera.current.transform.forward, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(Camera.current.transform.position, Camera.current.transform.forward, out hit))
        {
            gen_pos = hit.point;
        }
        else
        {
            gen_pos = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
#endif
#if UNITY_ANDROID
        {
            Destroy(this.gameObject);
        }
#endif



