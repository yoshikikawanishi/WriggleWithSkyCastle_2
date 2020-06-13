using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SEManager))]
public class SEManagerEditor : Editor {

    public override void OnInspectorGUI() {
        var obj = target as SEManager;

        for(int i = 0; i < obj.SE_List.Count; i++) {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            obj.SE_List[i].name = EditorGUILayout.TextField("", obj.SE_List[i].name);
            EditorGUILayout.BeginHorizontal();
            obj.SE_List[i].clip = (AudioClip)EditorGUILayout.ObjectField("", obj.SE_List[i].clip, typeof(AudioClip), false);
            GUILayout.FlexibleSpace();
            obj.SE_List[i].volume = EditorGUILayout.FloatField("Volume", obj.SE_List[i].volume);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", GUILayout.Width(100), GUILayout.Height(15))) {
                obj.Remove_SE(i);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        //追加ボタン
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add", GUILayout.Width(150), GUILayout.Height(20))) {
            obj.Add_SE();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorUtility.SetDirty(obj);
    }
}
