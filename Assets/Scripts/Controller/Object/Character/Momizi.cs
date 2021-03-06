﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Momizi : TalkCharacter {

    [SerializeField] private RopeWay rop_Way;

    new void Start() {
        base.Start();
        CollectionManager c = CollectionManager.Instance;
        if(c.Is_Collected("Momizi") && c.Is_Collected("Aya")) {
            if (SceneManager.GetActiveScene().name == "Stage3_3Scene") {
                Change_Status_With_Aya();
            }
            else {
                Destroy(gameObject);
            }
        }
    }


    protected override float Action_Before_Talk() {
        //ロープウェイが届いたらセリフと表情変える
        if (start_ID == 1) {
            if (rop_Way.transform.position.x > 2200f && rop_Way.transform.position.y > -16f) {
                Change_Message_Status("MomiziText", 2, 4);
                GetComponent<Animator>().SetTrigger("GladTrigger");
            }
        }
        return 0;
    }


    protected override void Action_In_End_Talk() {
        //ロープウェイ到着セリフ後
        if (start_ID == 2) {
            Change_Message_Status("MomiziText", 5, 9);            
            Put_Out_Collection_Box();
        }
    }


    private void Change_Status_With_Aya() {
        transform.position = new Vector3(6640f, -50f);
        transform.localScale = new Vector3(-1, 1, 1);
        GetComponent<Animator>().SetTrigger("GladTrigger");
        Change_Message_Status("AyaText", 53, 58);
    }
}
