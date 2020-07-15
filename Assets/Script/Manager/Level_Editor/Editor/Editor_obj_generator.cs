using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(obj_Generator))]
public class Editor_obj_generator : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        obj_Generator generator = (obj_Generator)target;

        if(GUILayout.Button("Generate Object"))
        {
            generator.Editor_generate();
        }
        if(GUILayout.Button("Read And Generate All"))
        {
            generator.ReadAndGenerateAll();
        }

    }
}
