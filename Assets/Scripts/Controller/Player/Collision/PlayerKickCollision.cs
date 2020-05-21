using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickCollision : MonoBehaviour {

    private CircleCollider2D _collider;
    private GameObject player;

    public bool is_Hit_Kick = false;    
    private Vector2 offset;

    private readonly string HIT_EFFECT_NAME = "HitEffect";  //PlayerAttackCollisionの方でオブジェクトプール
    private List<string> hit_Attack_Tag_List = new List<string> {
        "EnemyTag",
        "SandbackTag",
        "SandbackGroundTag"
    };


    private void Start() {
        _collider = GetComponent<CircleCollider2D>();
        player = transform.parent.gameObject;
    }

    //衝突判定
    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in hit_Attack_Tag_List) {
            if (collision.tag == tag && !is_Hit_Kick) {
                is_Hit_Kick = true;                
                Play_Hit_Effect();
            }
        }
    }

    //衝突検知用
    public bool Hit_Trigger() {
        if (is_Hit_Kick) {
            is_Hit_Kick = false;
            return true;
        }
        return false;
    }   


    //当たり判定を生成する
    public void Make_Collider_Appear(bool is_Charge_Kick) {
        _collider.enabled = true;
        Play_Animation(is_Charge_Kick);
    }

    //当たり判定を消す
    public void Make_Collider_Disappear() {
        _collider.enabled = false;
        Stop_Animation();
    }

    //キックのアニメーション再生
    private void Play_Animation(bool is_Charge_Kick) {
        if (is_Charge_Kick) {
            transform.GetChild(0).GetComponent<Animator>().SetBool("ChargeKickBool", true);
        }
        int power = PlayerManager.Instance.Get_Power();        
        if (power >= 64) {
            transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool2", true);            
        }
        else if (power >= 32) {
            transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool1", true);
        }
    }

    private void Stop_Animation() {
        transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool1", false);
        transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool2", false);
        transform.GetChild(0).GetComponent<Animator>().SetBool("ChargeKickBool", false);
    }


    //衝突時のエフェクト
    private void Play_Hit_Effect() {
        GameObject effect = ObjectPoolManager.Instance.Get_Pool(HIT_EFFECT_NAME).GetObject();
        offset = new Vector2(_collider.offset.x * transform.parent.localScale.x, _collider.offset.y);
        effect.transform.position = transform.position + (Vector3)offset;
        ObjectPoolManager.Instance.Set_Inactive(effect, 1.5f);
    }


    //当たり判定の範囲を取得
    public Vector2[] Get_Collision_Range() {
        Vector2[] range = new Vector2[2];
        Vector2 center = transform.position;
        center += new Vector2(_collider.offset.x * player.transform.localScale.x, _collider.offset.y);
        Vector2 diff = new Vector2(1, 1) * _collider.radius;
        range[0] = center - diff;
        range[1] = center + diff;
        return range;
    }

}
