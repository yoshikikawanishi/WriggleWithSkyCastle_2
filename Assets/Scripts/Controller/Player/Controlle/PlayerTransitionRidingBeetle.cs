using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionRidingBeetle : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;

    //向き
    private int scale_X = 1;


	// Use this for initialization
	void Start () {
        _rigid = GetComponent<Rigidbody2D>();
	}

    //移動
    public void Transition(Vector2 direction) {
        if (Time.timeScale == 0) return;

        //移動
        _rigid.velocity = direction * 250f;              
        //向き
        if(transform.localScale.x != scale_X) {
            transform.localScale = new Vector3(scale_X, 1, 1);
        }
    }


    //向きの変更
    public void Change_Body_Direction(int scale_X) {
        scale_X = scale_X > 0 ? 1 : -1;
        this.scale_X = scale_X;
    }

    //Getter
    public int Get_Beetle_Direction() {
        return scale_X;
    }

}
