using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIConflictBehaviour))]
public class UIConflictBehaviourEditor : Editor
{
    private SerializedProperty _conflictWithAllProp;
    private SerializedProperty _solveConflictBehaviourProp;
    private SerializedProperty _conflictsProp;
    private SerializedProperty _exclusionConflictsProp;
    
    private void OnEnable()
    {
        _conflictWithAllProp = serializedObject.FindProperty("_conflictWithAll");
        _solveConflictBehaviourProp = serializedObject.FindProperty("_solveConflictBehaviour");
        _conflictsProp = serializedObject.FindProperty("_conflicts");
        _exclusionConflictsProp = serializedObject.FindProperty("_exclusionConflicts");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_conflictWithAllProp);
        
        if (_conflictWithAllProp.boolValue)
        {
            EditorGUILayout.PropertyField(_solveConflictBehaviourProp);
            EditorGUILayout.PropertyField(_exclusionConflictsProp, true);
        }
        else
        {
            EditorGUILayout.PropertyField(_conflictsProp, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
