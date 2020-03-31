using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoCollision : BossCollisionDetection {
    
    private bool damaged_Trigger = false;
    private int damaged_Trigger_Count = 0;    

    protected override void Damaged(string key) {
        base.Damaged(key);
        if(key != "PlayerBulletTag")
            damaged_Trigger = true;
    }


    private void Update() {
        //被弾後3フレーム後にdamaged_Triggerをfalseにする
        if (damaged_Trigger) {
            damaged_Trigger_Count++;
            if (damaged_Trigger_Count > 3) {
                damaged_Trigger = false;
                damaged_Trigger_Count = 0;
            }
        }
    }

    /// <summary>
    /// 被弾を検知する
    /// </summary>
    /// <returns></returns>
    public bool Damaged_Trigger() {
        if (damaged_Trigger) {
            damaged_Trigger = false;
            return true;
        }
        return false;
    }
}
