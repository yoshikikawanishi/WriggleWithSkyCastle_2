using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yamame : TalkCharacter {

    new void Start() {
        base.Start();
        if(CollectionManager.Instance.Is_Collected("Yamame") && CollectionManager.Instance.Is_Collected("Kisume")) {
            if(SceneManager.GetActiveScene().name == "Stage4_2Scene")
                Change_Status_With_Kisume();
        }
    }


    protected override float Action_Before_Talk() {
        if (SceneManager.GetActiveScene().name == "Stage4_2Scene")
            return 0;
        if (talk_Count == 1)
            Change_Message_Status("YamameText", 1, 7);
        else
            Change_Message_Status("YamameText", 8, 10);
        return base.Action_Before_Talk();
    }


    protected override void Action_In_End_Talk() {
        base.Action_In_End_Talk();
        if (SceneManager.GetActiveScene().name == "Stage4_2Scene")
            return;
        if (transform.childCount > 1) {
            var box = transform.GetChild(0);
            box.SetParent(null);
            box.gameObject.SetActive(true);
        }
    }


    private void Change_Status_With_Kisume() {        
        transform.position = new Vector3(3820f, -84f);
        Change_Message_Status("YamameText", 11, 15);
    }
}
