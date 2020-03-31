using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFairyJump : MonoBehaviour {

    //ジャンプ力
    [SerializeField] private Vector2 jump_Velocity = new Vector2(50f, 180f);
    //当たり判定検知用
    private ChildColliderTrigger foot_Collision;
    private ChildColliderTrigger side_Collision;

    //コンポーネント
    private Rigidbody2D _rigid;

    //画面内に移るまで待機
    private bool wait_Visible = true;
    //現在の方向
    private int direction = -1;
    //初期値
    private Vector2 default_Scale;


    private void Start() {
        //取得
        foot_Collision = transform.GetChild(0).GetComponent<ChildColliderTrigger>();
        side_Collision = transform.GetChild(1).GetComponent<ChildColliderTrigger>();
        _rigid = GetComponent<Rigidbody2D>();
        default_Scale = transform.localScale;
    }
 
	
	// Update is called once per frame
	void Update () {
        if (wait_Visible)
            return;

        //ジャンプ
        if (foot_Collision.Hit_Trigger()) {
            _rigid.velocity = new Vector2(transform.localScale.x * -jump_Velocity.x, jump_Velocity.y);
        }
        //壁で反転
        if (side_Collision.Hit_Trigger()) {
            _rigid.velocity = new Vector2(-_rigid.velocity.x, _rigid.velocity.y);
            direction = _rigid.velocity.x.CompareTo(0);
            if (direction == 0) direction = 1;
            transform.localScale = new Vector3(-default_Scale.x * direction, default_Scale.y);
        }
        //落下時消滅
        if (transform.position.y < -200f) {
            Destroy(gameObject);
        }
	}


    private void OnBecameVisible() {
        wait_Visible = false;
        _rigid.gravityScale = 32f;
    }

}
