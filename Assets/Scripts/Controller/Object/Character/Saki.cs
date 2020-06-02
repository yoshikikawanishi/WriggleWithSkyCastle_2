using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saki : TalkCharacter {

    [SerializeField] private GameObject collection_Box;

    protected override void Action_In_End_Talk() {
        base.Action_In_End_Talk();
        if(talk_Count == 1) {
            collection_Box.SetActive(true);
            Change_Message_Status("SakiText", 1, 1);
        }
    }

}
