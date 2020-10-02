using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yamame : TalkCharacter {

    protected override void Action_In_End_Talk() {
        base.Action_In_End_Talk();
        if(transform.childCount > 1) {
            var box = transform.GetChild(0);
            box.SetParent(null);
            box.gameObject.SetActive(true);
        }
    }

}
