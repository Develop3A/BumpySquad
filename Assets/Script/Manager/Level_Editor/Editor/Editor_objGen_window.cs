using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Editor_objGen_window : EditorWindow
{
    string filename;
    bool isSaveLoadMode = true;

    string object_name;
    Editor gameObjectEditor;
    GameObject search_name;

    [MenuItem("Window/ -- Object Spawner --")]

    static void ShowWindow()
    {
        GetWindow<Editor_objGen_window>("Obj Spawner");
        var window = GetWindow<Editor_objGen_window>();
        window.maxSize = window.minSize = new Vector2(300, 300);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("저장/불러오기 감추기");
        if(isSaveLoadMode)
        {
            if(GUILayout.Button("감추기"))
            {
                isSaveLoadMode = false;
            }
        }
        else
        {
            if (GUILayout.Button("펼치기"))
            {
                isSaveLoadMode = true;
            }
        }
        EditorGUILayout.EndHorizontal();
        if (isSaveLoadMode)
        {
            EditorGUILayout.BeginHorizontal();
            filename = EditorGUILayout.TextField("ㄴ세이브명", filename);
            bool load = GUILayout.Button("불러오기");
            bool save = GUILayout.Button("저장");
            if (save)
            {
                obj_Save saver = FindObjectOfType<obj_Save>();
                saver.filename = filename;
                saver.Write();
            }
            if (load)
            {
                obj_Generator generator = FindObjectOfType<obj_Generator>();
                generator.filename = filename;
                generator.Load(filename);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        object_name = EditorGUILayout.TextField("오브젝트 이름으로 검색", object_name);
        search_name = (GameObject)objNumberManager.instance.Search_Prefab(object_name);
        GameObject obj = (GameObject)EditorGUILayout.ObjectField(search_name, typeof(GameObject), true);
        if (obj != null)
        {
            if (gameObjectEditor == null)
            {
                gameObjectEditor = Editor.CreateEditor(obj);
            }
            else
            {
                if (gameObjectEditor.name != obj.name)
                {
                    gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(150, 150), GUIStyle.none);
                    //GetWindow<Editor_objGen_window>().Repaint();
                }
            }
            if (GUILayout.Button("Generate"))
            {
                FindObjectOfType<obj_Generator>().Editor_generate(obj);
                //g.transform.position = 
            }
            }
        else
        {
            if (gameObjectEditor != null)
            {
                gameObjectEditor = null;
            }
        }
    }

}
