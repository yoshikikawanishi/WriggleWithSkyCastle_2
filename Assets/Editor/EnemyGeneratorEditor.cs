using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyGenerator))]
public class EnemyGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        EnemyGenerator obj = target as EnemyGenerator;

        //基本設定
        obj.gen_Only_Flying = EditorGUILayout.Toggle("Gen_Only_Flying", obj.gen_Only_Flying);
        obj.parent = (Transform)EditorGUILayout.ObjectField("Parent", obj.parent, typeof(Transform), true);        
        obj.generator_Parent = (Transform)EditorGUILayout.ObjectField("GeneratorParent", obj.generator_Parent, typeof(Transform), true);
        EditorGUILayout.Space();
        obj.start_Gen_Distance_From_Camera = EditorGUILayout.FloatField("Start_Gen_Distance", obj.start_Gen_Distance_From_Camera);
        obj.num = EditorGUILayout.IntField("Num", obj.num);
        obj.span = EditorGUILayout.FloatField("Span", obj.span);
        obj.position_Noise = EditorGUILayout.Vector2Field("Position_Noise", obj.position_Noise);

        //生成後の移動操作
        obj.is_Controlle_Move = EditorGUILayout.Toggle("Controlle_Move", obj.is_Controlle_Move);
        if (obj.is_Controlle_Move) {
            obj.x_Move = EditorGUILayout.CurveField("X_Move", obj.x_Move);
            obj.y_Move = EditorGUILayout.CurveField("Y_Move", obj.y_Move);
            obj.is_End_And_Delete = EditorGUILayout.Toggle("End_And_Delete", obj.is_End_And_Delete);
        }

        EditorUtility.SetDirty(obj);
    }

}
