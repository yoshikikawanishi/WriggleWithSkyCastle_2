using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saki : TalkCharacter {

    [SerializeField] private GameObject collection_Box;

    protected override float Action_Before_Talk() {
        if(talk_Count == 2)
            Change_Message_Status("SakiText", 3, 6);
        else if(talk_Count == 3)
            Change_Message_Status("SakiText", 7, 10);
        return 0;
    }

    protected override void Action_In_End_Talk() {
        base.Action_In_End_Talk();
        if(talk_Count == 1) {
            collection_Box.SetActive(true);            
        }
    }

}
