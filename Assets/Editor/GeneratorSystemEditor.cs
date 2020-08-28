using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneratorSystem))]
[CanEditMultipleObjects]
public class GeneratorSystemEditor : Editor {

    public override void OnInspectorGUI() {
        GeneratorSystem obj = target as GeneratorSystem;

        //追加ボタン
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add", GUILayout.Width(160f), GUILayout.Height(15f))) {
            obj.Add_List(0);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        for (int i = 0; i < obj.list.Count; i++) {

            EditorGUILayout.BeginVertical(GUI.skin.box);

            obj.list[i].name = EditorGUILayout.TextField("", obj.list[i].name, GUILayout.Height(20));
            obj.list[i].kind = (GeneratorSystem.Kind)EditorGUILayout.EnumPopup("Kind", obj.list[i].kind);
            obj.list[i].obj = (GameObject)EditorGUILayout.ObjectField("Obj", obj.list[i].obj, typeof(GameObject), false);
            obj.list[i].parent = (GameObject)EditorGUILayout.ObjectField("Parent", obj.list[i].parent, typeof(GameObject), true);
            obj.list[i].is_Object_Pool = EditorGUILayout.Toggle("ObjectPool", obj.list[i].is_Object_Pool);

            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Space(5);

            obj.list[i].num = EditorGUILayout.IntField("Num", obj.list[i].num);

            obj.list[i].pos_Kind = (GeneratorSystem.PosKind)EditorGUILayout.EnumPopup("Position Type", obj.list[i].pos_Kind);

            if (obj.list[i].kind == GeneratorSystem.Kind.liner) {
                obj.list[i].liner.initial_Pos = EditorGUILayout.Vector2Field("InitialPos", obj.list[i].liner.initial_Pos);
                if (obj.list[i].num > 1)
                    obj.list[i].liner.inter_Vector = EditorGUILayout.Vector2Field("InterSpace", obj.list[i].liner.inter_Vector);
            }
            else if(obj.list[i].kind == GeneratorSystem.Kind.rotate) {
                obj.list[i].rotate.center_Pos = EditorGUILayout.Vector2Field("Center", obj.list[i].rotate.center_Pos);
                EditorGUILayout.BeginHorizontal();
                {
                    obj.list[i].rotate.radius = EditorGUILayout.FloatField("Radius", obj.list[i].rotate.radius);
                    if(obj.list[i].num > 1)
                        obj.list[i].rotate.spread_Radius = EditorGUILayout.FloatField("SpreadRad", obj.list[i].rotate.spread_Radius);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    obj.list[i].rotate.initial_Angle_Deg = EditorGUILayout.FloatField("InitialAngle[deg]", obj.list[i].rotate.initial_Angle_Deg);
                    if (obj.list[i].num > 1)
                        obj.list[i].rotate.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle[deg]", obj.list[i].rotate.inter_Angle_Deg);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (obj.list[i].num > 1)
                obj.list[i].span = EditorGUILayout.FloatField("Span[s]", obj.list[i].span);

            obj.list[i].after_Span = EditorGUILayout.FloatField("AfterSpan[s]", obj.list[i].after_Span);
            
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));            

            //削除ボタン
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();            
            if (GUILayout.Button("Remove", GUILayout.Width(160f), GUILayout.Height(15f))) {
                obj.Remove_List(i);
            }            
            GUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            //追加ボタン
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();            
            if (GUILayout.Button("Add", GUILayout.Width(160f), GUILayout.Height(15f))) {
                obj.Add_List(i + 1);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }

        EditorUtility.SetDirty(obj);
    }

}
