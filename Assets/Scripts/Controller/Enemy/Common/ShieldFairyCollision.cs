using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFairyCollision : EnemyCollisionDetection {


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //盾で防ぐ
        float distance = collision.transform.position.x - transform.position.x;
        float height = collision.transform.position.y - transform.position.y;
        if(distance * transform.localScale.x.CompareTo(0) < -8 && height < 28f) {
            StartCoroutine("Block");
            return;
        }
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.tag == key) {
                Damaged(key);
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        //盾で防ぐ
        float distance = collision.transform.position.x - transform.position.x;
        float height = collision.transform.position.y - transform.position.y;
        if (distance * transform.localScale.x.CompareTo(0) < -8 && height < 28f) {
            StartCoroutine("Block");
            return;
        }
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.gameObject.tag == key) {
                Damaged(key);
            }
        }
    }


    private IEnumerator Block() {
        //衝撃
        GetComponent<Rigidbody2D>().velocity = new Vector2(100f * transform.localScale.x, 0);
        //点滅
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.2f);
        _sprite.color = new Color(0.5f, 0.5f, 0.5f);
    }

}
