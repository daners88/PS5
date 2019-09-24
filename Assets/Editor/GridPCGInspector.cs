using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridPCG))]
public class GridPCGInspector : Editor
{
    public override void OnInspectorGUI()
    {

        GridPCG pcg = target as GridPCG;

        if (GUILayout.Button("Clear"))
        {
            pcg.Clear();
        }

        if (GUILayout.Button("Build"))
        {
            pcg.Build();
        }

        base.OnInspectorGUI();
    }
}
