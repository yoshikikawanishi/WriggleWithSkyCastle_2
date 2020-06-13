using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MelodyManager))]
public class BGMMelodyEditor : Editor {
    

    public override void OnInspectorGUI() {
        MelodyManager obj = target as MelodyManager;

        for(int i = 0; i < obj.melody_List.Count; i++) {            
            obj.melody_List[i].melody = (MelodyManager.Melody)EditorGUILayout.EnumPopup("Melody", obj.melody_List[i].melody);
            obj.melody_List[i].span = EditorGUILayout.Vector2Field("Span[s]", obj.melody_List[i].span);

            if(i > 0) {
                obj.melody_List[i].span = new Vector2(obj.melody_List[i - 1].span.y, obj.melody_List[i].span.y);
            }
            if(obj.melody_List[i].span.x > obj.melody_List[i].span.y) {
                obj.melody_List[i].span = new Vector2(1, 1) * obj.melody_List[i].span.x;
            }

            EditorGUILayout.Space();
        }

        //メロディー追加
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add", GUILayout.Width(200), GUILayout.Height(20))) {
            obj.Add_Melody();
        }
        if(GUILayout.Button("Remove", GUILayout.Width(200), GUILayout.Height(20))){
            obj.Remove_Melody();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

}
