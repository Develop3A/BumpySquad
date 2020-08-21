using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Enemy_Save : CsvReader
{
    void Start()
    {
        //Write();
    }

    public override void Write()
    {
        StreamWriter sw;
        sw = new StreamWriter("Assets/Resources/" + filename + ".csv", false, System.Text.Encoding.Default);

        List<GameObject> enemies = new List<GameObject>();
        GameObject objs_gameobject = GameObject.FindWithTag("enemies");

        sw.WriteLine("obj_name,transform_parent,position.x,position.y,position.z,rotation.x,rotation.y,rotation.z,scale.x,scale.y,scale.z");
        foreach (GameObject enemy in enemies)
        {
            List<string> values = new List<string>();
            //A 
            string[] name = enemy.name.Split('(');
            values.Add(name[0]);

            //B
            string parent_tag;
            parent_tag = "objs";
            //if (parent_tag == "Untagged") parent_tag = obj.transform.parent.name;
            values.Add(parent_tag);

            //C D E
            values.Add(enemy.transform.position.x.ToString());
            values.Add(enemy.transform.position.y.ToString());
            values.Add(enemy.transform.position.z.ToString());

            //F G H
            values.Add(enemy.transform.eulerAngles.x.ToString());
            values.Add(enemy.transform.eulerAngles.y.ToString());
            values.Add(enemy.transform.eulerAngles.z.ToString());

            //I J K
            values.Add(enemy.transform.localScale.x.ToString());
            values.Add(enemy.transform.localScale.y.ToString());
            values.Add(enemy.transform.localScale.z.ToString());

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
