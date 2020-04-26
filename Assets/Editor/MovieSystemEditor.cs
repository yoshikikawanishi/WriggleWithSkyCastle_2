using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovieSystem))]
[CanEditMultipleObjects]
public class MovieSystemEditor : Editor {
    

    public override void OnInspectorGUI() {
        
        MovieSystem obj = target as MovieSystem;

        //イベント追加ボタン
        {
            if (GUILayout.Button("Add Event", GUILayout.Width(80), GUILayout.Height(25))) {
                obj.Add_Event(0);
            }
        }       

        //イベントリスト
        for (int i = 0; i < obj.list.Count; i++) {
            obj.list[i].type = (MovieSystem.Event.Type)EditorGUILayout.EnumPopup("type", obj.list[i].type);

            switch (obj.list[i].type) {
                case MovieSystem.Event.Type.message:
                    obj.list[i].message.file_Name = EditorGUILayout.TextField("filename", obj.list[i].message.file_Name);
                    obj.list[i].message.start_ID = EditorGUILayout.IntField("startID", obj.list[i].message.start_ID);
                    obj.list[i].message.end_ID = EditorGUILayout.IntField("endID", obj.list[i].message.end_ID);
                    obj.list[i].message.is_Auto = EditorGUILayout.Toggle("Auto Message", obj.list[i].message.is_Auto);
                    break;
                case MovieSystem.Event.Type.motion:
                    obj.list[i].motion.obj = (GameObject)EditorGUILayout.ObjectField("Object", obj.list[i].motion.obj, typeof(GameObject), true);
                    obj.list[i].motion.x_Move = EditorGUILayout.CurveField("X Move", obj.list[i].motion.x_Move);
                    obj.list[i].motion.y_Move = EditorGUILayout.CurveField("Y Move", obj.list[i].motion.y_Move);
                    obj.list[i].motion.anim_Parameter = EditorGUILayout.TextField("Animation Parameter", obj.list[i].motion.anim_Parameter);
                    obj.list[i].motion.param_Bool = EditorGUILayout.Toggle("Parameter Bool", obj.list[i].motion.param_Bool);
                    break;
                case MovieSystem.Event.Type.genObj:
                    obj.list[i].gen_Obj.obj = (GameObject)EditorGUILayout.ObjectField("Object", obj.list[i].gen_Obj.obj, typeof(GameObject), true);
                    obj.list[i].gen_Obj.parent = (GameObject)EditorGUILayout.ObjectField("Parent", obj.list[i].gen_Obj.parent, typeof(GameObject), true);
                    obj.list[i].gen_Obj.position = EditorGUILayout.Vector2Field("Position", obj.list[i].gen_Obj.position);
                    break;
                case MovieSystem.Event.Type.wait:
                    obj.list[i].wait.wait_Message = EditorGUILayout.Toggle("Wait End Message", obj.list[i].wait.wait_Message);
                    obj.list[i].wait.wait_Time = EditorGUILayout.FloatField("Time", obj.list[i].wait.wait_Time);
                    break;
            }

            //イベント削除ボタン  
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove Event", GUILayout.Width(80), GUILayout.Height(15))) {
                obj.Remove_Event(i);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            //追加ボタン
            if (GUILayout.Button("Add Event", GUILayout.Width(80), GUILayout.Height(25))) {
                obj.Add_Event(i + 1);
            }                            

            EditorGUILayout.Space();
        }

        EditorUtility.SetDirty(obj);        

    }    

}
