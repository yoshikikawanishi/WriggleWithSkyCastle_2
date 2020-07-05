using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RopeWay : MonoBehaviour {

    [SerializeField] private List<Vector2> move_Distance_And_Direction;
    [SerializeField] private bool is_Stop_In_End = false;

    private List<string> moved_Tags = new List<string> {
        "PlayerKickTag",
    };

    private Animator _anim;    

    private float move_Speed = 0;
    private float now_Location = 0;
    private int now_List_Index = 0;    


	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();        
	}
	

    private void FixedUpdate() {
        
        //次のインデックスに進む
        if (now_Location > Vector2_Abs(move_Distance_And_Direction[now_List_Index])){
            if (now_List_Index < move_Distance_And_Direction.Count - 1) {
                now_Location = 0;
                now_List_Index++;
            }
            //終着点
            else if (move_Speed > 0){                                                   
                //終着点で機能を停止する
                if (is_Stop_In_End) {
                    this.enabled = false;
                    _anim.SetBool("MoveBool", false);
                    _anim.SetBool("BackBool", false);
                }
                //それ以外の時は後ろに下がる
                move_Speed -= 0.03f;
                return;
            }
        }
        //前のインデックスに戻る
        if(now_Location < 0) {
            if(now_List_Index > 0) {
                now_List_Index--;
                now_Location = Vector2_Abs(move_Distance_And_Direction[now_List_Index]);                
            }
            //スタート地点
            else if (move_Speed < 0){
                move_Speed = 0;
                _anim.SetBool("BackBool", false);                
            }
        }
        //移動
        transform.position += (Vector3)(move_Speed * move_Distance_And_Direction[now_List_Index].normalized);
        now_Location += move_Speed;
        //減速、停止後は一定速度で後ろに戻る
        if (move_Speed < 0) {
            move_Speed = -0.5f;
            _anim.SetBool("MoveBool", false);
            _anim.SetBool("BackBool", true);
        }
        else {
            move_Speed -= 0.03f;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (this.enabled == false)
            return;

        foreach(string tag in moved_Tags) {
            if(tag == collision.tag) {
                move_Speed = 1.5f;
                _anim.SetBool("MoveBool", true);
                _anim.SetBool("BackBool", false);
                Play_Hit_Effect();
            }
        }
    }


    //Vector2の絶対値
    private float Vector2_Abs(Vector2 vec) {
        float x_2 = Mathf.Pow(vec.x, 2);
        float y_2 = Mathf.Pow(vec.y, 2);
        return Mathf.Sqrt(x_2 + y_2);
    }


    //エフェクト
    private void Play_Hit_Effect() {
        GetComponentInChildren<ParticleSystem>().Play();        
    }

}
