using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {

    //Inspecter上で編集すること
    [SerializeField] private List<string> delete_Bullet_Obj_Tag = new List<string> {
        "EnemyTag"
    };

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in delete_Bullet_Obj_Tag) {
            if(collision.tag == tag) {
                gameObject.SetActive(false);
            }
        }
    }

    //OnBecameInvisible
    private void OnBecameInvisible() {
        gameObject.SetActive(false);
    }
}
