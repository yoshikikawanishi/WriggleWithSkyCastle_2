using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shizuha : TalkCharacter {

    [SerializeField] private Minoriko minoriko;

    private new void Start() {
        base.Start();
        //アイテム取得済みの時
        if (CollectionManager.Instance.Is_Collected("Shizuha")) {
            Change_Message_Status("ShizuhaText", 8, 12);
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position = new Vector3(4168f, transform.position.y);
        }
    }

    protected override float Action_Before_Talk() {
        if(minoriko.is_Defeated && start_ID == 1) {
            Change_Message_Status("ShizuhaText", 3, 3);
        }
        return 0;
    }


    protected override void Action_In_End_Talk() {
        //穣子撃破後の会話後、アイテムを出す
        if (start_ID == 3) {
            base.Put_Out_Collection_Box();
            Change_Message_Status("ShizuhaText", 4, 7);
        }
    }
}
