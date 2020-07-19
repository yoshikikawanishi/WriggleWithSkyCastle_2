using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rumia : TalkCharacter {

    //ルーミアの状態
    //PlayerPrefs.GetInt("Rumia") == (null, 未発見 / 1, ルーミアだけ発見済み / 2, ミスティア発見済み)

    private GameObject player;

    private bool is_Waiting = true;


    private new void Start() {
        base.Start();
        player = GameObject.FindWithTag("PlayerTag");                

        mark_Up_Baloon.transform.SetParent(null);
    }


    private void Update() {
        if (is_Waiting)
            return;
        //常に自機の方を向く
        transform.localScale = new Vector3(Compare_Player_Position(), 1, 1);        
    }


    //自機が右にいるとき１、左にいるとき-1を返す
    private int Compare_Player_Position() {
        if((player.transform.position.x - transform.position.x).CompareTo(0) < 0) {
            return -1;
        }
        return 1;
    }


    protected override float Action_Before_Talk() {
        GetComponent<ParticleSystem>().Stop();
        is_Waiting = false;

        //初回時は固定
        if (talk_Count == 1) {
            Change_Message_Status("RumiaText", 2, 4);
            return 0f;
        }
        //未発見
        int rumia = PlayerPrefs.GetInt("Rumia");
        if (rumia == 0) {            
            Change_Message_Status("RumiaText", 2, 4);
        }
        //ルーミアだけ発見済み
        else if(rumia == 1) {
            Change_Message_Status("RumiaText", 5, 5);
        }
        //ミスティアが隣にいるとき
        else if (rumia == 2) {
            Change_Status_With_Mystia();
        }
        
        return 0f;
    }    


    protected override void Action_In_End_Talk() {    
        //未発見だった時発見済みに
        if (PlayerPrefs.GetInt("Rumia") == 0) {
            PlayerPrefs.SetInt("Rumia", 1);
            //宝箱出す           
            var box = transform.GetChild(0).gameObject;
            box.transform.position = new Vector3(766f, -19f);
            box.transform.SetParent(null);
            box.SetActive(true);           
        }
        //初回時会話方法変更
        if(talk_Count == 1) {            
            talk_Type = TalkType.upperArrow;
            mark_Up_Baloon.SetActive(true);
        }

    }    


    //ミスティアが横にいる時のステータスに変更
    private void Change_Status_With_Mystia() {
        GetComponent<ParticleSystem>().Stop();
        Change_Message_Status("RumiaText", 6, 6);
    }

}
