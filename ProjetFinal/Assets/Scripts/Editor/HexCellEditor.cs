using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCell)), CanEditMultipleObjects]
public class HexCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tile = target as HexCell;

        DrawDefaultInspector();

        GUILayout.Label("\n");

        if (GUILayout.Button("Update Tile (CTRL + S after click)"))
        {
            foreach (Transform t in tile.transform.parent)
            {
                if (t.GetComponent<HexCell>())
                {
                    t.GetComponent<HexCell>().UpdateTileDatas(tile.tileType);
                    EditorUtility.SetDirty(t);
                }
            }
        }
    }
}