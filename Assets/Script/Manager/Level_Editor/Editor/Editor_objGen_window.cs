using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_objGen_window : EditorWindow
{
    string object_name;
    Editor gameObjectEditor;
    GameObject search_name;

    [MenuItem("Window/ -- Object Spawner --")]

    static void ShowWindow()
    {
        GetWindow<Editor_objGen_window>("Obj Spawner");
        var window = GetWindow<Editor_objGen_window>();
        window.maxSize = window.minSize = new Vector2(300, 220);
    }

    static void Open()
    {
        var window = GetWindow<Editor_objGen_window>();
        window.maxSize = window.minSize = new Vector2(300, 250);
    }

    void OnGUI()
    {
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
                GameObject g = FindObjectOfType<obj_Generator>().Editor_generate(obj);
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
