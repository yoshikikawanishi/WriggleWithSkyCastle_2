using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Parameter {
    public string comment;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1.0f, 1.0f);
    public float speed = 0.01f;
    public float arc_Size = 0;
    public bool is_Local_Position = false;

    public Parameter() {

    }
}


public class MoveTwoPoints : MonoBehaviour {

    [SerializeField] private List<Parameter> param = new List<Parameter> { new Parameter() };

    private int index = 0;
    private bool end_Move = false;
   

    //移動開始
    public void Start_Move(Vector3 next_Pos) {
        end_Move = false;
        index = 0;
        StopCoroutine("Move_Two_Points");
        StartCoroutine("Move_Two_Points", next_Pos);
    }

    public void Start_Move(Vector3 next_Pos, int index) {
        end_Move = false;
        this.index = index;
        StopCoroutine("Move_Two_Points");
        StartCoroutine("Move_Two_Points", next_Pos);
    }


    //移動終了
    public void Stop_Move() {
        StopCoroutine("Move_Two_Points");
    }


    //移動用のコルーチン
    private IEnumerator Move_Two_Points(Vector3 next_Pos) {        
        float now_Location  = 0;        //現在の移動距離割合
        float now_Time = 0;             //現在の時間進行度
        Vector3 start_Pos = transform.position;
        Vector3 pos = start_Pos;

        while (now_Location < 1) {
            now_Time += param[index].speed;
            now_Location = param[index].curve.Evaluate(now_Time);
            pos = Vector3.Lerp(start_Pos, next_Pos, now_Location);  //直線の軌道
            pos += new Vector3(0, param[index].arc_Size * Mathf.Sin(now_Location * Mathf.PI), 0); //弧の軌道
            if (param[index].is_Local_Position) {
                transform.localPosition = pos;               
            }
            else {
                transform.position = pos;
            }            
            yield return new WaitForSeconds(0.016f);
        }

        end_Move = true;
    }


    //移動終了検知用
    public bool End_Move() {
        if (end_Move) {
            return true;
        }        
        return false;        
    }


    //ステータスの変更
    public void Change_Paramter(float speed, float arc_Size, int index) {
        this.param[index].speed = speed;
        this.param[index].arc_Size = arc_Size;
    }

    //速度変化の変更
    public void Change_Transition_Curve(AnimationCurve curve, int index) {
        this.param[index].curve = curve;
    }
}
