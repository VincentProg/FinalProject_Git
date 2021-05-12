using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HexGrid))]
public class EditorTools : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        HexGrid objTarget = (HexGrid)target;
        if (GUILayout.Button("Generate HexMap"))
        {
            objTarget.Generate();
        }
    }

}
