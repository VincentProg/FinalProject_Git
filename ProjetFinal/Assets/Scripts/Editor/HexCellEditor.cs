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
                    tile.GetComponent<HexCell>().UpdateTileDatas(tile.tileType);
                    EditorUtility.SetDirty(tile);
        }

        if (GUILayout.Button("Update all sprites"))
        {

            foreach (Transform child in FindObjectOfType<HexGrid>().transform)
            {
                if (child.GetComponent<HexCell>() != null)
                {
                   
                    HexCell script = child.GetComponent<HexCell>();
                    TileSaver tileSaver = child.GetComponent<TileSaver>();
                    script.coordinates = tileSaver.coordinates;
                    script.tileType = tileSaver.type;
                    script.UpdateTileDatas(script.tileType);
                    child.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = (script.coordinates.Y - script.coordinates.X) - 100;
                    EditorUtility.SetDirty(child);
                }
            }
        }
    }
}