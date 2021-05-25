using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSaver))]
public class EditorTileSaver : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        GUILayout.Label("\n");

        if (GUILayout.Button("Save all tiles in tileSaver"))
        {
            foreach (Transform child in FindObjectOfType<HexGrid>().transform)
            {
                if (child.GetComponent<HexCell>() != null)
                {
                    HexCell script = child.GetComponent<HexCell>();
                    TileSaver tileSaver = child.GetComponent<TileSaver>();
                    tileSaver.coordinates = script.coordinates;
                    tileSaver.type = script.tileType;
                    EditorUtility.SetDirty(child);
                }
            }
        }
    }
}
