using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(obj_Generator))]
public class Editor_obj_generator : Editor {

    string filename_;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        obj_Generator generator = (obj_Generator)target;

        if(GUILayout.Button("Generate Object"))
        {
            generator.Editor_generate();
        }
        EditorGUILayout.BeginHorizontal();
        filename_ = EditorGUILayout.TextField("Filename", filename_);
        bool isButton = GUILayout.Button("Load");
        EditorGUILayout.EndHorizontal();
        if (isButton) {
            isButton = false;
        generator.Load(filename_);
        }

    }
}
