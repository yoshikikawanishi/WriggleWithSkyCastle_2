using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFairy : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;

    private bool wait_Visible = true;
    private int direction = -1;
    

	// Use this for initialization
	void Start () {
        //取得
        _rigid = GetComponent<Rigidbody2D>();        
    }


    // Update is called once per frame
    void Update() {
        if (wait_Visible) 
            return;
        
        //歩く
        _rigid.velocity = new Vector2(direction * 40, _rigid.velocity.y);
        
        //落下時消す
        if(transform.position.y < -170f) {
            Destroy(gameObject);
        }       
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        //反転
        if(collision.tag == "InvisibleWallTag") {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            direction *= -1;
        }
    }


    private void OnBecameVisible() {
        wait_Visible = false;
    }

}
