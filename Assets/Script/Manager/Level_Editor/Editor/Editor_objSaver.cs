using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(obj_Save))]
public class Editor_objSaver : Editor
{
    string filename_;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        obj_Save saver = (obj_Save)target;

        EditorGUILayout.BeginHorizontal();
        filename_ = EditorGUILayout.TextField("Filename",filename_);
        bool isButton = GUILayout.Button("Save");
        EditorGUILayout.EndHorizontal();

        if (isButton)
        {
            isButton = false;
            saver.Set_filename(filename_);
            saver.Write();
        }

    }
}
