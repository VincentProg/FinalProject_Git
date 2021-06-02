using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Attack))]
public class AttackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var attack = target as Attack;


        attack.name = EditorGUILayout.TextField("Name", attack.name);
        GUILayout.Label("Sprite");
        attack.sprite = (Sprite)EditorGUILayout.ObjectField(attack.sprite, typeof(Sprite), allowSceneObjects: true);

        attack.throwingSound = EditorGUILayout.TextField("Nom du son au lancer", attack.throwingSound);
        attack.activationSound = EditorGUILayout.TextField("Nom du son a l'activation", attack.activationSound);

        GUILayout.Label("\n");
        attack.costPA = EditorGUILayout.IntField("Cost PA", attack.costPA);
        attack.cooldown = EditorGUILayout.IntField("Cooldown", attack.cooldown);
        attack.nbrUsePerTurn = EditorGUILayout.IntField("Number Use/Turn", attack.nbrUsePerTurn);
        attack.nbrUseTotal = EditorGUILayout.IntField("Number Use Total", attack.nbrUseTotal);


        GUILayout.Label("\n \n");
        attack.rangeType = (Attack.RANGE_TYPE)EditorGUILayout.EnumPopup("Type of Range", attack.rangeType);
        attack.impactType = (Attack.IMPACT_TYPE)EditorGUILayout.EnumPopup("Type of Impact", attack.impactType);

        if (attack.rangeType == Attack.RANGE_TYPE.OWNCELL)
        {

            GUILayout.Label("\n IMPACT DATAS");
            switch (attack.impactType)
            {
                case Attack.IMPACT_TYPE.POINT:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    break;
                case Attack.IMPACT_TYPE.LINES:
                    GUILayout.Label("RANGE.OWNCELL et IMPACT.LINE ne sont pas compatibles");
                    break;
                case Attack.IMPACT_TYPE.ARC:
                    GUILayout.Label("RANGE.OWNCELL et IMPACT.ARC ne sont pas compatibles");
                    break;
                case Attack.IMPACT_TYPE.RADIUS:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    attack.rangeImpact = EditorGUILayout.IntField("Range Impact", attack.rangeImpact);
                    attack.radiusUnattackableImpact = EditorGUILayout.IntField("Range UnImpactable", attack.radiusUnattackableImpact);
                    break;
                case Attack.IMPACT_TYPE.SPAWNOBJECT:
                    GUILayout.Label("RANGE.OWNCELL et IMPACT.SPAWNOBJECT ne sont pas compatibles");
                    break;
            }
        } else if (attack.rangeType == Attack.RANGE_TYPE.LINE)
        {
            GUILayout.Label("\n RANGE DATAS");
            attack.rangeAttack = EditorGUILayout.IntField("Range Attack", attack.rangeAttack);
            attack.radiusUnattackableAttack = EditorGUILayout.IntField("Range Unattackable", attack.radiusUnattackableAttack);
            attack.visionType = (Attack.VISION_TYPE)EditorGUILayout.EnumPopup("Vision Type", attack.visionType);
            attack.canSelectHole = EditorGUILayout.Toggle("Can Select Hole", attack.canSelectHole);
            attack.canSelectHeroEnemySpawner = EditorGUILayout.Toggle("Can Select Hero-Enemy-Spawner", attack.canSelectHeroEnemySpawner);



            GUILayout.Label("\n IMPACT DATAS");
            switch (attack.impactType)
            {
                case Attack.IMPACT_TYPE.POINT:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    break;
                case Attack.IMPACT_TYPE.LINES:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    attack.rangeImpact = EditorGUILayout.IntField("Range Impact", attack.rangeImpact);
                    attack.radiusUnattackableImpact = EditorGUILayout.IntField("Range UnImpactable", attack.radiusUnattackableImpact);
                    break;
                case Attack.IMPACT_TYPE.LINE:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    attack.rangeImpact = EditorGUILayout.IntField("Range Impact", attack.rangeImpact);
                    break;
                case Attack.IMPACT_TYPE.ARC:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    break;
                case Attack.IMPACT_TYPE.RADIUS:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    attack.rangeImpact = EditorGUILayout.IntField("Range Impact", attack.rangeImpact);
                    attack.radiusUnattackableImpact = EditorGUILayout.IntField("Range UnImpactable", attack.radiusUnattackableImpact);
                    break;
                case Attack.IMPACT_TYPE.SPAWNOBJECT:
                    attack.spawnObject = (GameObject)EditorGUILayout.ObjectField("Object", attack.spawnObject, typeof(GameObject), allowSceneObjects: true);
                    break;
            }
        }
        else
        {
            GUILayout.Label("\n RANGE DATAS");
            attack.rangeAttack = EditorGUILayout.IntField("Range Attack", attack.rangeAttack);
            attack.radiusUnattackableAttack = EditorGUILayout.IntField("Range Unattackable", attack.radiusUnattackableAttack);
            attack.visionType = (Attack.VISION_TYPE)EditorGUILayout.EnumPopup("Vision Type", attack.visionType);
            attack.canSelectHole = EditorGUILayout.Toggle("Can Select Hole", attack.canSelectHole);
            attack.canSelectHeroEnemySpawner = EditorGUILayout.Toggle("Can Select Hero-Enemy-Spawner", attack.canSelectHeroEnemySpawner);

            GUILayout.Label("\n IMPACT DATAS");
            switch (attack.impactType)
            {
                case Attack.IMPACT_TYPE.POINT:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    break;
                case Attack.IMPACT_TYPE.LINES:
                    GUILayout.Label("RANGE.RADIUS et IMPACT.LINE ne sont pas compatibles");
                    break;
                case Attack.IMPACT_TYPE.ARC:
                    GUILayout.Label("RANGE.RADIUS et IMPACT.ARC ne sont pas compatibles");
                    break;
                case Attack.IMPACT_TYPE.RADIUS:
                    attack.damages = EditorGUILayout.IntField("Amount Damages", attack.damages);
                    attack.rangeImpact = EditorGUILayout.IntField("Range Impact", attack.rangeImpact);
                    attack.radiusUnattackableImpact = EditorGUILayout.IntField("Range UnImpactable", attack.radiusUnattackableImpact);
                    break;
                case Attack.IMPACT_TYPE.SPAWNOBJECT:
                    attack.spawnObject = (GameObject)EditorGUILayout.ObjectField("Object", attack.spawnObject, typeof(GameObject), allowSceneObjects: true);
                    break;
            }
        }


        GUILayout.Label("\n \n \n");

        if(GUILayout.Button("Save Changes (Do Ctrl+S after click)"))
        {
            EditorUtility.SetDirty(target);
        }
    }

    
}
