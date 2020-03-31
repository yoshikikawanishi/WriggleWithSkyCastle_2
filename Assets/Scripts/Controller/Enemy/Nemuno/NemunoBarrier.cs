using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoBarrier : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    //ネムノ本体
    private GameObject nemuno;

    private bool is_Barrier = false;

    private List<string> blink_Tag_List = new List<string> {
        "PlayerBulletTag",        
        "PlayerAttackTag",
        "PlayerChargeAttackTag",        
        "PlayerKickTag",
    };


    private void Awake() {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        nemuno = transform.parent.gameObject;
    }


    private void LateUpdate() {
        //バリア発動中
        if (is_Barrier) {
            if(_sprite.color.a < 0.9f) {
                _sprite.color += new Color(0, 0, 0, 0.02f);
            }
            else if(!_collider.enabled){
                _collider.enabled = true;
            }
            transform.Rotate(new Vector3(0, 0, 1f * Time.timeScale));
            transform.position = nemuno.transform.position;
        }
        //ばリア停止中
        else {
            if (_collider.enabled) {
                _collider.enabled = false;
            }
            if(_sprite.color.a > 0) {
                _sprite.color -= new Color(0, 0, 0, 0.02f);
            }
            else {
                gameObject.SetActive(false);
                transform.SetParent(nemuno.transform);  //親子関係を戻す
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in blink_Tag_List) {
            if(collision.tag == tag) {
                StartCoroutine("Blink");
            }
        }
    }


    //バリアの生成
    public void Start_Barrier() {
        gameObject.SetActive(true);     //生成
        transform.SetParent(null);      //被弾判定を取らないために親子判定を切る
        transform.position = nemuno.transform.position;         //座標
        _sprite.color = _sprite.color * new Color(1, 1, 1, 0);  //透明にする
        _collider.enabled = false;
        is_Barrier = true;
    }

    //バリアを消す
    public void Stop_Barrier() {
        is_Barrier = false;
    }


    private IEnumerator Blink() {
        _sprite.color = new Color(0.8f, 0.8f, 0.8f, _sprite.color.a);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(0.5f, 0.5f, 0.5f, _sprite.color.a);
    }
}
