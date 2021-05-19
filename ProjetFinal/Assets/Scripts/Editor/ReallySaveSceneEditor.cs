using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ReallySaveScene))]
public class ReallySaveSceneEditor : Editor
{
    public override void OnInspectorGUI()
    { 
        if (GUILayout.Button("Save Scene (CTRL + S after click)"))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(currentScene);
        }
    }
}



