using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCell))]
public class HexCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tile = target as HexCell;

        DrawDefaultInspector();

        GUILayout.Label("\n");

        if (GUILayout.Button("Update Tile (CTRL + S after click)"))
        {
            tile.UpdateTileDatas(tile.tileType);
            EditorUtility.SetDirty(target);
        }
    }
}