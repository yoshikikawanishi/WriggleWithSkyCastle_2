using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAroundMotion : MonoBehaviour {

    [SerializeField] private float speed = 10f;
    [SerializeField] private float radius = 16f;
    [SerializeField] private float start_Angle = 180f;

    private bool is_End_Motion = false;


    /// <summary>
    /// center_Pos中心にrotate_Deg度回転する
    /// </summary>
    /// <param name="center_Pos"></param>
    /// <param name="rotate_Deg"></param>
    public void Start_Motion(Vector2 center_Pos, float rotate_Deg) {
        StopAllCoroutines();
        is_End_Motion = false;
        StartCoroutine(Go_Around_Cor(center_Pos, rotate_Deg));
    }


    private IEnumerator Go_Around_Cor(Vector2 center_Pos, float rotate_Deg) {
        float angle = start_Angle;

        for (int i = 0; i < rotate_Deg / Mathf.Abs(speed); i++) {
            transform.position = center_Pos + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            angle += speed;
            yield return new WaitForSeconds(0.016f);
        }

        is_End_Motion = true;
    }


    /// <summary>
    /// 終了を検知する用
    /// </summary>
    /// <returns></returns>
    public bool Is_End_Motion() {
        if (is_End_Motion) {
            is_End_Motion = false;
            return true;
        }
        return false;
    }


    /// <summary>
    /// 回転を止める
    /// </summary>
    public void Stop_Motion() {
        StopAllCoroutines();
    }


    //Setter
    public void Set_Speed(float speed) {
        this.speed = speed;
    }

    public void Set_Radius(float radius) {
        this.radius = radius;
    }

    public void Set_Start_Angle(float start_Angle) {
        this.start_Angle = start_Angle;
    }

}
