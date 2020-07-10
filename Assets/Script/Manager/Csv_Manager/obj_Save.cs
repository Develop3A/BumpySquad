using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class obj_Save : CsvReader
{
    public int row_count = 11;
    public GameObject[] save;
    public bool act =false;

    void Update()
    {
        if(act)
        {
            act = false;
            Write();
        }
    }

    void Start()
    {
        //Write();
    }

    public override void Write()
    {
        StreamWriter sw;
        sw = new StreamWriter("Assets/Resources/" + filename + ".csv", false, System.Text.Encoding.Default);

        List<GameObject> objs = new List<GameObject>();

        foreach(GameObject g in save) //objs 에 세이브할 오브젝트들 추가
        {
            if (g.transform.childCount == 0) continue;

            for(int child_num =0; child_num<g.transform.childCount;child_num++)
            {
                objs.Add(g.transform.GetChild(child_num).gameObject);
            }
        }

        sw.WriteLine("obj_name,transform_parent,position.x,position.y,position.z,rotation.x,rotation.y,rotation.z,scale.x,scale.y,scale.z");
        foreach (GameObject obj in objs)
        {
            List<string> values = new List<string>();
            //A 
            string[] name = obj.name.Split('(');
            values.Add(name[0]);

            //B
            string parent_tag;
            parent_tag = obj.transform.parent.tag;
            if (parent_tag == "Untagged") parent_tag = obj.transform.parent.name;
            values.Add(parent_tag);

            //C D E
            values.Add(obj.transform.position.x.ToString());
            values.Add(obj.transform.position.y.ToString());
            values.Add(obj.transform.position.z.ToString());

            //F G H
            values.Add(obj.transform.eulerAngles.x.ToString());
            values.Add(obj.transform.eulerAngles.y.ToString());
            values.Add(obj.transform.eulerAngles.z.ToString());

            //I J K
            values.Add(obj.transform.localScale.x.ToString());
            values.Add(obj.transform.localScale.y.ToString());
            values.Add(obj.transform.localScale.z.ToString());

            string result= "";
            for(int i=0; i<values.Count; i++)
            {
                if (i != 0)
                {
                    result += ",";
                }

                result += values[i];
            }
            sw.WriteLine(result);
        }

        sw.Close();
    }
}
