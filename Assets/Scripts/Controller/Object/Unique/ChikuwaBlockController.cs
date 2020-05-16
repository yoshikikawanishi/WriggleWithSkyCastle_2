using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChikuwaBlockController : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;

    //自機が乗っている時間
    private float stay_Time = 0;
    //自機が乗っているか
    private bool is_Standing_Player = false;


    //start
    private void Start() {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
    }


    private void Update() {
        if (is_Standing_Player) {
            if(stay_Time < 1.0f) {
                stay_Time += Time.deltaTime;
            }
        }
        if(stay_Time >= 1.0f) {
            transform.position -= new Vector3(0, 1f, 0) * Time.timeScale;
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            is_Standing_Player = true;
            //色を変える
            _sprite.color = new Color(1, 0.7f, 0.7f);
        }
        //地面をすり抜け
        else if (collision.tag == "GroundTag") {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerFootTag") {
            is_Standing_Player = false;
            //色を戻す
            _sprite.color = new Color(1, 1, 1);
        }    
        //地面のすり抜けをなくす
        else if(collision.tag == "GroundTag") {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

}
