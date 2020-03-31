using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momizi : TalkCharacter {

    [SerializeField] private RopeWay rop_Way;

    new void Start() {
        base.Start();
        CollectionManager c = CollectionManager.Instance;
        if(c.Is_Collected("Momizi") && c.Is_Collected("Aya")) {
            Change_Status_With_Aya();
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
        transform.position = new Vector3(5740f, -64f);
        Change_Message_Status("MomiziText", 10, 10);
    }
}
