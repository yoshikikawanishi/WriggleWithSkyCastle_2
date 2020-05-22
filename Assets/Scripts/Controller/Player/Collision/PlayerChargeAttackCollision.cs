using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttackCollision : MonoBehaviour {

    private bool is_Hit_Attack = false;

    private List<string> hit_Attack_Tag_List = new List<string> {
        "EnemyTag",
        "SandbackTag",
        "SandbackGroundTag"
    };


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in hit_Attack_Tag_List) {
            if (collision.tag == tag && !is_Hit_Attack) {
                is_Hit_Attack = true;
            }
        }
    }


    public bool Hit_Trigger() {
        if (is_Hit_Attack) {
            is_Hit_Attack = false;
            return true;
        }
        return false;
    }


    //攻撃の生成
    public void Make_Collider_Appear(float lifeTime) {
        is_Hit_Attack = false;
        GetComponent<BoxCollider2D>().enabled = true;
        Play_Animation();        
        Invoke("Make_Collider_Disappear", lifeTime);
    }


    public void Make_Collider_Disappear() {
        GetComponent<BoxCollider2D>().enabled = false;
    }


    //アニメーション再生
    private void Play_Animation() {
        GetComponent<Animator>().SetTrigger("AttackTrigger3");
    }   


    private void Set_Size(float scale, Vector2 position) {
        int direction = transform.localScale.x.CompareTo(0);
        transform.localScale = new Vector3(direction, 1, 1) * scale;
        transform.localPosition = position;
    }
}
