using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollisionDetection : EnemyCollisionDetection {    

    protected BossEnemy _boss_Enemy;
    

    void Awake() {
        _boss_Enemy = GetComponent<BossEnemy>();
        //ダメージ量変更        
        damaged_Tag_Dictionary["PlayerAttackTag"] = 30;
        damaged_Tag_Dictionary["PlayerKickTag"] = 30;
        damaged_Tag_Dictionary["PlayerChargeAttackTag"] = 50;        
    }


    //被弾の処理
    protected override void Damaged(string key) {
        //ダメージの計算
        int damage = (int)(damaged_Tag_Dictionary[key] * Damage_Rate());
        _boss_Enemy.Damaged(damage, key);
        //自機の緑ゲージ回復
        if (key != "PlayerBulletTag") {
            BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 20);
        }
    }


    //無敵化
    public void Become_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }

    //無敵化解除
    public void Release_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }

}
