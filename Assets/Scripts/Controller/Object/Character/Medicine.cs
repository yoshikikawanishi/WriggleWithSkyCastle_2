using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : TalkCharacter {

    private new void Start() {
        base.Start();
        //アイテム取得済みで幽香撃破済みの場合
        if (CollectionManager.Instance.Is_Collected("Medicine") && PlayerPrefs.GetInt("YukaTutorial") == 2) {            
            Change_Status_With_Yuka();
        }
    }


    protected override void Action_In_End_Talk() {
        if (CollectionManager.Instance.Is_Collected("Medicine"))
            return;
        Put_Out_Collection_Box();
    }


    //幽香の隣に移動
    private void Change_Status_With_Yuka() {
        transform.position = new Vector3(2060f, -109f);
        transform.localScale = new Vector3(-1, 1, 1);
        Change_Message_Status("MedicineText", 5, 5);
    }
    
}
