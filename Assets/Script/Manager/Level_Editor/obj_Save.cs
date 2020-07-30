using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class obj_Save : CsvReader
{
    void Start()
    {
        //Write();
    }

    public override void Write()
    {
        StreamWriter sw;
        sw = new StreamWriter("Assets/Resources/" + filename + ".csv", false, System.Text.Encoding.Default);

        List<GameObject> objs = new List<GameObject>();
        GameObject objs_gameobject = GameObject.FindWithTag("objs");
        Save_Childs(objs_gameobject, objs);

        sw.WriteLine("obj_name,transform_parent,position.x,position.y,position.z,rotation.x,rotation.y,rotation.z,scale.x,scale.y,scale.z");
        foreach (GameObject obj in objs)
        {
            List<string> values = new List<string>();
            //A 
            string[] name = obj.name.Split('(');
            values.Add(name[0]);

            //B
            string parent_tag;
            parent_tag = "objs";
            //if (parent_tag == "Untagged") parent_tag = obj.transform.parent.name;
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
            Debug.Log(result);
            sw.WriteLine(result);
        }

        sw.Close();
    }

    public void Save_Childs(GameObject parent_obj,List<GameObject> objs)
    {
        for (int child_num = 0; child_num < parent_obj.transform.childCount; child_num++)
        {
            objs.Add(parent_obj.transform.GetChild(child_num).gameObject);
            if(parent_obj.transform.GetChild(child_num).childCount >0)
            {
                Save_Childs(parent_obj.transform.GetChild(child_num).gameObject, objs);
            }
        }
    }
}
