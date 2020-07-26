using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovieSystem))]
[CanEditMultipleObjects]
public class MovieSystemEditor : Editor {

    public int component_Popup_Index = 0;
    public int method_Popup_Index = 0;
    

    public override void OnInspectorGUI() {
        
        MovieSystem obj = target as MovieSystem;

        obj.is_Disable_Controlle = EditorGUILayout.Toggle("DisableControlle", obj.is_Disable_Controlle);

        //イベント追加ボタン
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("AddEvent", GUILayout.Width(160), GUILayout.Height(15))) {
                obj.Add_Event(0);
            }
            GUILayout.FlexibleSpace();            
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        //イベントリスト        
        for (int i = 0; i < obj.list.Count; i++) {

            EditorGUILayout.BeginHorizontal();            
            obj.list[i].is_Open_In_Inspector = EditorGUILayout.Toggle("", obj.list[i].is_Open_In_Inspector, GUILayout.Width(10));
            obj.list[i].name = EditorGUILayout.TextField("", obj.list[i].name);
            GUILayout.FlexibleSpace();
            if (!obj.list[i].is_Open_In_Inspector) {
                EditorGUILayout.EndHorizontal();
                continue;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            //obj.list[i].name = EditorGUILayout.TextArea(obj.list[i].name);
            obj.list[i].type = (MovieSystem.Event.Type)EditorGUILayout.EnumPopup("type", obj.list[i].type);

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
           
            //イベントごとの値
            switch (obj.list[i].type) {
                case MovieSystem.Event.Type.message:                    
                    obj.list[i].message.file_Name = EditorGUILayout.TextField("filename", obj.list[i].message.file_Name);                    
                    obj.list[i].message.id = EditorGUILayout.Vector2IntField("ID", obj.list[i].message.id);
                    obj.list[i].message.dual_Panel = EditorGUILayout.Toggle("DualPanel", obj.list[i].message.dual_Panel);
                    obj.list[i].message.is_Auto = EditorGUILayout.Toggle("Auto Message", obj.list[i].message.is_Auto);
                    break;
                case MovieSystem.Event.Type.motion:                    
                    obj.list[i].motion.obj = (Transform)EditorGUILayout.ObjectField("Object", obj.list[i].motion.obj, typeof(Transform), true);
                    obj.list[i].motion.apply_Root_Position = EditorGUILayout.Toggle("ApplyRootPosition", obj.list[i].motion.apply_Root_Position);                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Curve");
                    EditorGUILayout.LabelField("X", GUILayout.Width(10));                    
                    obj.list[i].motion.x_Move = EditorGUILayout.CurveField("", obj.list[i].motion.x_Move, GUILayout.Width(100));
                    EditorGUILayout.LabelField("Y", GUILayout.Width(10));
                    obj.list[i].motion.y_Move = EditorGUILayout.CurveField("", obj.list[i].motion.y_Move, GUILayout.Width(100));                    
                    EditorGUILayout.EndHorizontal();
                    break;
                case MovieSystem.Event.Type.animation:
                    obj.list[i].animation.kind = (MovieSystem.Animation.ParameterKind)EditorGUILayout.EnumPopup("Kind", obj.list[i].animation.kind);
                    obj.list[i].animation.animator = (Animator)EditorGUILayout.ObjectField("Object", obj.list[i].animation.animator, typeof(Animator), true);
                    EditorGUILayout.BeginHorizontal();
                    obj.list[i].animation.parameter = EditorGUILayout.TextField("Parameter", obj.list[i].animation.parameter, GUILayout.Width(400));
                    GUILayout.FlexibleSpace();
                    if (obj.list[i].animation.kind == MovieSystem.Animation.ParameterKind.boolean)
                        obj.list[i].animation.boolean = EditorGUILayout.Toggle("", obj.list[i].animation.boolean);
                    EditorGUILayout.EndHorizontal();
                    break;
                case MovieSystem.Event.Type.localScale:
                    obj.list[i].localScale.obj = (Transform)EditorGUILayout.ObjectField("Object", obj.list[i].localScale.obj, typeof(Transform), true);
                    obj.list[i].localScale.kind = (MovieSystem.LocalScale.Kind)EditorGUILayout.EnumPopup("Kind", obj.list[i].localScale.kind);
                    if(obj.list[i].localScale.kind == MovieSystem.LocalScale.Kind.direct) {
                        obj.list[i].localScale.scale = EditorGUILayout.Vector2Field("Scale", obj.list[i].localScale.scale);
                    }
                    else {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Curve");
                        EditorGUILayout.LabelField("X", GUILayout.Width(10));
                        obj.list[i].localScale.x_Curve = EditorGUILayout.CurveField("", obj.list[i].localScale.x_Curve, GUILayout.Width(100));
                        EditorGUILayout.LabelField("Y", GUILayout.Width(10));
                        obj.list[i].localScale.y_Curve = EditorGUILayout.CurveField("", obj.list[i].localScale.y_Curve, GUILayout.Width(100));
                        EditorGUILayout.EndHorizontal();
                    }
                    break;
                case MovieSystem.Event.Type.genObj:
                    obj.list[i].gen_Obj.obj = (GameObject)EditorGUILayout.ObjectField("Object", obj.list[i].gen_Obj.obj, typeof(GameObject), true);
                    obj.list[i].gen_Obj.parent = (GameObject)EditorGUILayout.ObjectField("Parent", obj.list[i].gen_Obj.parent, typeof(GameObject), true);
                    obj.list[i].gen_Obj.position = EditorGUILayout.Vector2Field("Position", obj.list[i].gen_Obj.position);
                    break;
                case MovieSystem.Event.Type.function:
                    GUILayout.BeginHorizontal();
                    obj.list[i].function.obj = (GameObject)EditorGUILayout.ObjectField("", obj.list[i].function.obj, typeof(GameObject), true);                    
                    if(obj.list[i].function.obj == null) {
                        GUILayout.EndHorizontal();
                        break;
                    }
                    //コンポーネントのポップアップ
                    MonoBehaviour[] components = obj.list[i].function.obj.GetComponents<MonoBehaviour>();
                    if (components.Length == 0) {
                        GUILayout.EndHorizontal();
                        break;
                    }                    
                    string[] names = new string[components.Length];                    
                    for (int j = 0; j < components.Length; j++)
                        names[j] = components[j].ToString();
                    component_Popup_Index = EditorGUILayout.Popup(component_Popup_Index, names);
                    obj.list[i].function.component = components[component_Popup_Index];
                    GUILayout.EndHorizontal();
                    //メソッドのポップアップ
                    Type t = obj.list[i].function.component.GetType();                    
                    MemberInfo[] methods = t.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);                    
                    if (methods.Length == 0)
                        break;
                    string[] method_Names = new string[methods.Length];
                    for (int j = 0; j < methods.Length; j++) 
                        method_Names[j] = methods[j].Name;                                            
                    method_Popup_Index = EditorGUILayout.Popup(method_Popup_Index, method_Names);
                    obj.list[i].function.function_Name = method_Names[method_Popup_Index];
                    break;
                case MovieSystem.Event.Type.wait:                    
                    obj.list[i].wait.wait_Message = EditorGUILayout.Toggle("Wait End Message", obj.list[i].wait.wait_Message);
                    obj.list[i].wait.wait_Time = EditorGUILayout.FloatField("Time", obj.list[i].wait.wait_Time);
                    break;
            }                       

            //イベント複製・並び替え・削除ボタン  
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();            
            if (GUILayout.Button("Up", GUILayout.Width(80), GUILayout.Height(15)) ) {
                obj.Swap_Event(i, i - 1);
            }
            if (GUILayout.Button("Down", GUILayout.Width(80), GUILayout.Height(15))) {
                obj.Swap_Event(i, i + 1);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(80), GUILayout.Height(15))) {
                obj.Remove_Event(i);
            }            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();            

            //追加ボタン
            if (i >= obj.list.Count)
                break;
            if (obj.list[i].type == MovieSystem.Event.Type.wait) {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("AddEvent", GUILayout.Width(160), GUILayout.Height(15))) {
                    obj.Add_Event(i + 1);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
            }
        
        }

        //追加ボタン
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("AddEvent", GUILayout.Width(160), GUILayout.Height(15))) {
            obj.Add_Event(obj.list.Count);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorUtility.SetDirty(obj);        

    }    

}
