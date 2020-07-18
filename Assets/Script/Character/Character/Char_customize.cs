using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Char_customize : EditorWindow
{

    public GameObject character;
    List<string> part_name = new List<string>();
    List<int> part_num = new List<int>();
    List<List<GameObject>> parts_list = new List<List<GameObject>>();

    [MenuItem("Window/ -- Character Customize --")]

    static void ShowWindow()
    {
        GetWindow<Char_customize>("Char Custom");
        var window = GetWindow<Char_customize>();
        window.maxSize = window.minSize = new Vector2(300, 250);
    }

    void OnGUI()
    {
        character = (GameObject)EditorGUILayout.ObjectField(character, typeof(GameObject), true);
        if (character)
        {
            if (parts_list.Count == 0) Init();
            for (int i = 0; i < parts_list.Count; i++)
            {
                Create_Button(i);
            }
        }
        else
        {
            if(parts_list.Count > 0)
            {
                parts_list.Clear();
                part_name.Clear();
                part_num.Clear();
                Debug.Log("clear");
            }
        }
    }
    
    public void Create_Button(int i)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(part_name[i]+" ["+ part_num[i]+"]");
        bool minus = GUILayout.Button("-");
        bool plus = GUILayout.Button("+");
        EditorGUILayout.EndHorizontal();
        if (plus)
        {
            plus = false;
            part_num[i]++;
            if(part_num[i] == parts_list[i].Count)
            {
                part_num[i] = -1;
            }
            for(int j =0; j< parts_list[i].Count;j++)
            {
                if(part_num[i] ==j)
                {
                    parts_list[i][j].SetActive(true);
                }
                else
                {
                    parts_list[i][j].SetActive(false);
                }
            }
        }
        if(minus)
        {
            minus = false;
            part_num[i]--;
            if (part_num[i] == -2)
            {
                part_num[i] = parts_list[i].Count-1;
            }
            for (int j = 0; j < parts_list[i].Count; j++)
            {
                if (part_num[i] == j)
                {
                    parts_list[i][j].SetActive(true);
                }
                else
                {
                    parts_list[i][j].SetActive(false);
                }
            }
        }
    }

    public void Init()
    {
        Add_parts(character.transform);
    }

    public void Add_parts(Transform parent)
    {
        List<GameObject> parts = new List<GameObject>();
        for (int i=0;i<parent.childCount;i++)
        {
            GameObject g = parent.GetChild(i).gameObject;
            if (g.transform.childCount > 0)
            {
                Add_parts(g.transform);
            }
            else
            {
                if(g.GetComponent<MeshRenderer>()||g.GetComponent<SkinnedMeshRenderer>())
                {
                    parts.Add(g.gameObject);
                }
            }
        }
        if (parts.Count > 0)
        {
            parts_list.Add(parts);
            part_name.Add(parent.name);
            part_num.Add(0);
        }
    }

}
