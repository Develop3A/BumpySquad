using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(obj_Save))]
public class Editor_objSaver : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        obj_Save saver = (obj_Save)target;

        if (GUILayout.Button("Save this Scene"))
        {
            saver.Write();
        }

    }
}
