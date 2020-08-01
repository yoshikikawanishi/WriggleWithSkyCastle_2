using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の被弾判定を取るクラス
/// </summary>
public class EnemyCollisionDetection : MonoBehaviour {

    protected Dictionary<string, int> damaged_Tag_Dictionary = new Dictionary<string, int>() {
        {"PlayerAttackTag"  , 12    },
        {"PlayerChargeAttackTag", 20 },        
        {"PlayerKickTag"    , 12    },
        {"PlayerBulletTag"  , 1     },
        {"PlayerTag"        , 10    },  
        {"PlayerBodyTag"    , 10    },
    };        

    protected Enemy _enemy;

    
    void Awake() {
        _enemy = GetComponent<Enemy>();
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.tag == key) {
                Damaged(key);
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.gameObject.tag == key) {
                Damaged(key);
            }
        }
    }


    //被弾の処理
    protected virtual void Damaged(string key) {
        if (_enemy == null)
            return;

        //ダメージの計算
        int damage = (int)(damaged_Tag_Dictionary[key] * Damage_Rate());
        _enemy.Damaged(damage, key);
        //自機の緑ゲージの回復
        if(key != "PlayerBulletTag") {
            BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 8);
        }
    }


    //自機のパワーに応じてダメージ増加
    protected float Damage_Rate() {
        int power = PlayerManager.Instance.Get_Power();
        if(power < 100) {
            return 1;
        }
        if(power < 200) {
            return 1.2f;
        }
        else if(power < 300) {
            return 1.5f;
        }
        else if(power < 400) {
            return 1.7f;
        }
        return 1.9f;
    }    
    
}
