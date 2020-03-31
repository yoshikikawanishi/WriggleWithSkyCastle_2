using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorikoTalk : TalkCharacter {

    protected override void Action_In_End_Talk() {
        if(start_ID == 1) {
            Change_Message_Status("MinorikoText", 6, 6);
        }
    }
}
