﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchFairy : FairyEnemy {

    private ChildColliderTrigger search_Light_Collider;

    private bool is_Searching = true;


    void Start() {
        search_Light_Collider = GetComponentInChildren<ChildColliderTrigger>();
    }

	
	//ライトが当たると戦闘開始
	void Update () {
        if (search_Light_Collider.Hit_Trigger() && is_Searching) {
            is_Searching = false;
            Start_Battle();
        }
	}


    //被弾すると戦闘開始
    public override void Damaged(int damage, string attacked_Tag) {
        base.Damaged(damage, attacked_Tag);
        if (is_Searching) {
            is_Searching = false;
            Start_Battle();
        }
    }


    //戦闘開始時の処理
    public void Start_Battle() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            return;

        GetComponent<WitchFairyBattleMovie>().Start_Battle_Movie(gameObject);
        Destroy(GetComponent<RedFairy>());
        Destroy(GetComponent<Rigidbody2D>());
    }
}
