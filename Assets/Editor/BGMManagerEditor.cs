using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BGMManager))]
public class BGMManagerEditor : Editor {


    public override void OnInspectorGUI() {
        BGMManager obj = target as BGMManager;        

        for (int i = 0; i < obj.BGM_List.Count; i++) {
            
            EditorGUILayout.LabelField(obj.BGM_List[i].name);

            EditorGUILayout.BeginHorizontal();
            {
                obj.BGM_List[i].have_Intoro = EditorGUILayout.Toggle("Intoro", obj.BGM_List[i].have_Intoro);
                if (obj.BGM_List[i].have_Intoro) {
                    obj.BGM_List[i].intoro_Clip = (AudioClip)EditorGUILayout.ObjectField("", obj.BGM_List[i].intoro_Clip, typeof(AudioClip), false);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                obj.BGM_List[i].clip = (AudioClip)EditorGUILayout.ObjectField("", obj.BGM_List[i].clip, typeof(AudioClip), false);
                obj.BGM_List[i].volume = EditorGUILayout.FloatField("volume", obj.BGM_List[i].volume);                
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
        }

        EditorUtility.SetDirty(obj);
        
    }

}
