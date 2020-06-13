using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve {
    public string comment;
    public AnimationCurve x_Curve;
    public AnimationCurve y_Curve;
    public bool apply_Root_Position;
}


public class MoveMotion : MonoBehaviour {

    [SerializeField] private List<Curve> pos_Curve = new List<Curve>();

    private bool is_End_Move = false;


    public void Start_Move() {
        is_End_Move = false;
        Stop_Move();
        StartCoroutine("Move", 0);
    }

    public void Start_Move(int index) {
        is_End_Move = false;
        Stop_Move();
        StartCoroutine("Move", index);
    }

    public void Stop_Move() {
        StopCoroutine("Move");
    }

    public bool Is_End_Move() {
        if (is_End_Move) {
            is_End_Move = false;
            return true;
        }
        return false;
    }


    private IEnumerator Move(int index) {
        AnimationCurve x_Curve = pos_Curve[index].x_Curve;
        AnimationCurve y_Curve = pos_Curve[index].y_Curve;
        bool apply_Root_Position = pos_Curve[index].apply_Root_Position;
        float time = 0;
        float end_Time = x_Curve.keys[x_Curve.length - 1].time;
        if (end_Time < y_Curve.keys[y_Curve.length - 1].time)
            end_Time = y_Curve.keys[y_Curve.length - 1].time;

        Vector3 root_Pos = transform.position;

        while (true) {
            if (apply_Root_Position) {
                transform.position = root_Pos + new Vector3(x_Curve.Evaluate(time), y_Curve.Evaluate(time));
            }
            else {
                transform.position = new Vector3(x_Curve.Evaluate(time), y_Curve.Evaluate(time));
            }
            time += Time.deltaTime;
            if (time >= end_Time) {
                break;
            }
            yield return null;
        }

        is_End_Move = true;
    }
    
}
