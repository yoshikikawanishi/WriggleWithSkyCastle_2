using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ShootSystemのインスペクタ表示の設定
/// </summary>
[CustomEditor(typeof(ShootSystem))]
[CanEditMultipleObjects]
public class ShootSystemEditer : Editor {

    public override void OnInspectorGUI() {        
        
        ShootSystem obj = target as ShootSystem;

        //コメント        
         obj.comment = EditorGUILayout.TextField("Comment", obj.comment);
        //基本設定
        obj.play_On_Awake = EditorGUILayout.Toggle("PlayOnAwake", obj.play_On_Awake);
        obj.bullet = (GameObject)EditorGUILayout.ObjectField("Bullet", obj.bullet, typeof(GameObject), false);
        obj.parent = (Transform)EditorGUILayout.ObjectField("Parent", obj.parent, typeof(Transform), true);
        obj.lifeTime = EditorGUILayout.FloatField("LifeTime", obj.lifeTime);                
        obj.offset = EditorGUILayout.Vector2Field("Offset", obj.offset);
        obj.default_Shoot_Sound = EditorGUILayout.Toggle("DefaultShootSound", obj.default_Shoot_Sound);

        EditorGUILayout.Space();

        //その他パラメータ
        obj.kind = (ShootSystem.KIND)EditorGUILayout.EnumPopup("Kind", obj.kind);        
        obj.other_Param = EditorGUILayout.Foldout(obj.other_Param, "Param");
        if (obj.other_Param == true) {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            obj.max_Speed = EditorGUILayout.FloatField("MaxSpeed", obj.max_Speed);
            obj.radius = EditorGUILayout.FloatField("Radius", obj.radius);

            switch (obj.kind) {
                case ShootSystem.KIND.Odd:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);                                       
                    break;

                case ShootSystem.KIND.Even:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);                    
                    break;

                case ShootSystem.KIND.Diffusion:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);                    
                    break;

                case ShootSystem.KIND.nWay:
                    obj.num = EditorGUILayout.IntField("Num", obj.num);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);                    
                    break;

                case ShootSystem.KIND.Scatter:
                    obj.shoot_Rate = EditorGUILayout.FloatField("Rate", obj.shoot_Rate);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.arc_Deg = EditorGUILayout.FloatField("Arc", obj.arc_Deg);
                    obj.duration = EditorGUILayout.FloatField("Duration", obj.duration);                    
                    break;

                case ShootSystem.KIND.Spiral:
                    obj.shoot_Rate = EditorGUILayout.FloatField("Rate", obj.shoot_Rate);
                    obj.inter_Angle_Deg = EditorGUILayout.FloatField("InterAngle_Deg", obj.inter_Angle_Deg);
                    obj.center_Angle_Deg = EditorGUILayout.FloatField("CenterAngle_Deg", obj.center_Angle_Deg);
                    obj.duration = EditorGUILayout.FloatField("Duration", obj.duration);                    
                    break;

            }
            EditorGUILayout.Space();
            obj.angle_Noise = EditorGUILayout.FloatField("AngleNoise_Deg", obj.angle_Noise);
            obj.speed_Noise = EditorGUILayout.FloatField("SpeedNoise", obj.speed_Noise);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        //弾の連結
        obj.connect_Bullet = EditorGUILayout.Toggle("ConnectBullet", obj.connect_Bullet);
        if (obj.connect_Bullet) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            obj.connect_Num = EditorGUILayout.IntField("ConnectNum", obj.connect_Num);            
            obj.speed_Diff = EditorGUILayout.FloatField("SpeedDiff", obj.speed_Diff);
            obj.angle_Diff = EditorGUILayout.FloatField("AngleDiff", obj.angle_Diff);
            EditorGUILayout.EndVertical();
        }        

        //ループ
        obj.looping = EditorGUILayout.Toggle("Looping", obj.looping);
        if (obj.looping) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            obj.loop_Count = EditorGUILayout.IntField("LoopCount", obj.loop_Count);
            obj.span = EditorGUILayout.FloatField("Span", obj.span);
            obj.center_Angle_Diff = EditorGUILayout.FloatField("Angle_Diff", obj.center_Angle_Diff);
            EditorGUILayout.EndVertical();
        }        

        //加速度
        obj.is_Acceleration = EditorGUILayout.Toggle("VelocityOverTime", obj.is_Acceleration);
        if (obj.is_Acceleration) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            if (obj.velocity_Forward == null) {
                obj.velocity_Forward = AnimationCurve.Linear(0, 0, obj.lifeTime, 1.0f);
                obj.velocity_Lateral = AnimationCurve.Linear(0, 0, obj.lifeTime, 1.0f);
            }
            obj.velocity_Forward = EditorGUILayout.CurveField("Forward", obj.velocity_Forward);
            obj.velocity_Lateral = EditorGUILayout.CurveField("Lateral", obj.velocity_Lateral);
            obj.velocity_To_Player = EditorGUILayout.CurveField("AimPlayer", obj.velocity_To_Player);            
            EditorGUILayout.EndVertical();
        }
        
        //角度の固定        
        obj.is_Fixed_Rotation = EditorGUILayout.Toggle("FixedRotation", obj.is_Fixed_Rotation);
        if (obj.is_Fixed_Rotation) {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            obj.fixed_Angle = EditorGUILayout.FloatField("FixedAngle", obj.fixed_Angle);
            EditorGUILayout.EndVertical();
        }
        

        //描画順
        obj.is_Change_Sorting_Order = EditorGUILayout.Toggle("ChangeSortingOrder", obj.is_Change_Sorting_Order);
        if (obj.is_Change_Sorting_Order) {
            obj.sorting_Order = EditorGUILayout.IntField("SortingOrder", obj.sorting_Order);
            obj.sorting_Order_Diff = EditorGUILayout.IntField("SortingOrder_Diff", obj.sorting_Order_Diff);
        }

        EditorUtility.SetDirty(obj);
    }    
    
}
