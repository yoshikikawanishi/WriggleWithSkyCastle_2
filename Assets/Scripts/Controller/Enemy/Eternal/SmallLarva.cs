using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLarva : Enemy {

    private Rigidbody2D _rigid;


    void Start() {
        _rigid = GetComponent<Rigidbody2D>();
    }


    public override void Damaged(int damage, string attacked_Tag) {
        //ショット無効
        if(attacked_Tag == "PlayerBulletTag") {
            StartCoroutine("White_Blink_Cor");
            return;
        }
        base.Damaged(damage, attacked_Tag);
    }


    void Update() {
        //方向転換
        if(_rigid.velocity.x > 0) {
            if(transform.localScale.x < 0) {
                transform.localScale = new Vector3(2, 2, 1);
            }
        }
        else {
            if(transform.localScale.x > 0) {
                transform.localScale = new Vector3(-2, 2, 1);
            }
        }
    }


    private IEnumerator White_Blink_Cor() {
        GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.8f, 0.9f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
    }
}
