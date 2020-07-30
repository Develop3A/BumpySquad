using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_Generator : CsvReader {

    [Header("생성할 오브젝트 이름")]
    [SerializeField] public string Objectname;
    string load_filename = "";
    Vector3 gen_pos;
    
    public override void Read()
    {
        base.Read();
        Set_filename(load_filename);

        List<GameObject> objects = new List<GameObject>();
        List<string> parent_name = new List<string>();

        for (int column = 1; column < all_words.GetLength(1); column++)
        {
            GameObject g = null;
            List<string> values = new List<string>();

            if (all_words[0, column] == "" | all_words[0, column] == null)
            {
                continue;
            }
            for (int row = 0; row < all_words.GetLength(0); row++)
            {
                //Debug.Log(all_words[row, column]);
                if (all_words[row, column] == "" | all_words[row, column] == null)
                    continue;
                else
                    values.Add(all_words[row, column]);
                //Debug.Log(all_words[row, column]);
            }
            

            //Pop<string>(values)
            string A = Pop<string>(values);
            GameObject ins = objNumberManager.instance.Find_Prefab(A);                              //A
            if (ins != null)
            {
                g = Instantiate(ins, null);
                //Debug.Log("a");
            }
            else
            {
                g = new GameObject();
                g.name = A;
                //Debug.Log("b");
            }

            string B = Pop<string>(values);
            parent_name.Add(B);                                                                    //B
            float x, y, z;

            x = float.Parse(Pop<string>(values));                                                  //C
            y = float.Parse(Pop<string>(values));                                                  //D
            z = float.Parse(Pop<string>(values));                                                  //E
            g.transform.position = new Vector3(x, y, z);

            x = float.Parse(Pop<string>(values));                                                  //F
            y = float.Parse(Pop<string>(values));                                                  //G
            z = float.Parse(Pop<string>(values));                                                  //H
            g.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));

            x = float.Parse(Pop<string>(values));                                                  //I
            y = float.Parse(Pop<string>(values));                                                  //J
            z = float.Parse(Pop<string>(values));                                                  //K
            g.transform.localScale = new Vector3(x, y, z);

            objects.Add(g);
            
            if (B == "objs") //부모의 이름이 objs라면
                g.transform.parent = GameObject.FindWithTag("objs").transform;
            /*
            else
            {//아니라면
                Debug.Log("123");
                GameObject search = GameObject.Find(parent_name); //우선 그 이름의 오브젝트가 있는지 검색
                if (search)
                {//있으면 그걸로
                    Debug.Log(search.name);
                    g.transform.parent = search.transform;
                }
                else
                {//없으면 에러
                    Debug.LogWarning("그럴일 없겠지만 부모 음슴");
                    g.transform.parent = GameObject.FindWithTag("objs").transform;
                }
            }
            */
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
    public void Load(string load_filename_)
    {
        load_filename = load_filename_;
        GameObject objs = GameObject.FindWithTag("objs");
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



