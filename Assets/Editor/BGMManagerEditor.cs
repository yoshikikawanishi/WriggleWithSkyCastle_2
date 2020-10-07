using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BGMManager))]
public class BGMManagerEditor : Editor {


    public override void OnInspectorGUI() {
        BGMManager obj = target as BGMManager;        

        for (int i = 0; i < obj.BGM_List.Count; i++) {
            EditorGUILayout.BeginHorizontal();
            {
                if(obj.BGM_List[i].is_Folding = EditorGUILayout.Toggle("", obj.BGM_List[i].is_Folding, GUILayout.Width(10))) {
                    EditorGUILayout.LabelField(obj.BGM_List[i].name);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
                    continue;
                }
                GUI.color = new Color(0.5f, 1f, 0.5f);
                obj.BGM_List[i].name = EditorGUILayout.TextField("", obj.BGM_List[i].name, GUILayout.Width(200), GUILayout.Height(15));
                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();

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

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Remove", GUILayout.Width(100), GUILayout.Height(10))) {
                    obj.Remove(i);
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
        }

        if (GUILayout.Button("Add", GUILayout.Width(200), GUILayout.Height(20))) {
            obj.Add_BGM();
        }

        EditorUtility.SetDirty(obj);
        
    }

}
